using System.Collections;
using UnityEngine;




[RequireComponent(typeof(AudioSource))]
public class LookTarget : MonoBehaviour
{
    public Transform target;
    public float rotSpeed = 5;
    public Transform baseRotation, head, eye;

    AudioSource rotSoundSource;
    [Range(0, 3)] public float pitchMin = 0.5f, pitchMax = 2;
    public float anglePitchMin = 3, anglePitchMax = 90;



    void OnDrawGizmosSelected()
    {
        float dist = 100;
        if (Physics.Raycast(eye.position, eye.forward, out RaycastHit hit, dist))
            dist = Tool.Dist(eye, hit.point);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eye.position, eye.position + eye.forward * dist);
    }


    void Awake()
    {
        rotSoundSource = GetComponent<AudioSource>();
        SetTarget(target, true);
    }


    private void OnEnable()
    {
        SetTarget(target, true);
    }


    public void SetTarget(Transform target, bool force = false)
    {
        if (!force && this.target == target)
            return;

        this.target = target;
        StopCoroutine("LookTargetCoroutine");
        StopCoroutine("LookForwardCoroutine");
        StartCoroutine(target ? "LookTargetCoroutine" : "LookForwardCoroutine");
    }


    IEnumerator LookTargetCoroutine()
    {
        while (target)
        {
            LookPoint(target.position);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine("LookForwardCoroutine");
    }



    IEnumerator LookForwardCoroutine()
    {
        while (Vector3.Angle(eye.forward, transform.forward) > 0.1f)
        {
            LookPoint(eye.position + transform.forward * 1000);
            yield return new WaitForEndOfFrame();
        }

        if (rotSoundSource.isPlaying)
            rotSoundSource.Stop();
    }


    void LookPoint(Vector3 pnt)
    {
        float anglePitch = 0;

        // rot baseRot
        Vector3 brPntTarget = baseRotation.InverseTransformPoint(pnt);
        brPntTarget.y = 0;
        anglePitch += Vector3.Angle(Vector3.forward, brPntTarget);
        brPntTarget = baseRotation.TransformPoint(brPntTarget);
        Quaternion brRotTarget = Quaternion.LookRotation(Tool.Dir(baseRotation, brPntTarget), baseRotation.up);
        baseRotation.rotation = Quaternion.Lerp(baseRotation.rotation, brRotTarget, rotSpeed * Time.deltaTime);

        // rot head
        Quaternion brRotMem = baseRotation.rotation;
        baseRotation.rotation = brRotTarget;
        Vector3 headPntTarget = pnt + Tool.Dir(eye, head);
        Vector3 headDirTarget = Tool.Dir(head, headPntTarget);
        anglePitch += Vector3.Angle(head.forward, headDirTarget);
        Quaternion headRotTarget = Quaternion.LookRotation(headDirTarget, baseRotation.up);
        head.rotation = Quaternion.Lerp(head.rotation, headRotTarget, rotSpeed * Time.deltaTime);
        baseRotation.rotation = brRotMem;

        // rot sound pitch
        if (anglePitch > anglePitchMin)
        {
            float t = Mathf.Clamp01((anglePitch - anglePitchMin) / (anglePitchMax - anglePitchMin));
            rotSoundSource.pitch = Mathf.Lerp(pitchMin, pitchMax, t);
            rotSoundSource.volume = t;

            if (!rotSoundSource.isPlaying)
                rotSoundSource.Play();
        }
        else if (rotSoundSource.isPlaying) rotSoundSource.Stop();
    }
}
