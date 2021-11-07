using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    public float view = 15;
    public LookTarget lookTarget;
    [Range(0, 180)] public float FOV = 30; // if lookTarget

    protected Transform target;

    public UnityEvent TargetEnterFOV, TargetExitFOV;
    bool targetInFOV = false;

    protected virtual void OnDrawGizmosSelected()
    {
        // draw view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, view);

        // draw fov
        if (lookTarget)
            Tool.GizmoDrawWireCone(lookTarget.eye.position, lookTarget.eye.forward, view, FOV);
    }


    private void OnDisable()
    {
        TargetExitFOV.Invoke();
        targetInFOV = false;
    }

    protected virtual void Update()
    {
        SearchTarget();

        if (targetInFOV != TargetInFOV())
        {
            targetInFOV = !targetInFOV;
            (targetInFOV ? TargetEnterFOV : TargetExitFOV).Invoke();
        }
    }



    void SearchTarget()
    {
        Enemy e = Enemy.GetClosest(transform.position);
        if (e && Tool.Dist(e, this) < view) SetTarget(e.transform);
        else SetTarget(null);
    }

    public void SetTarget(Transform target)
    {
        if (this.target == target)
            return;

        this.target = target;
        lookTarget?.SetTarget(target);
    }

    public bool TargetInFOV() => target && (!lookTarget || Vector3.Angle(lookTarget.eye.forward, Tool.Dir(lookTarget.eye, target)) < FOV / 2);
}
