using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[RequireComponent(typeof(Animator))]
public class TowerShoot : Tower
{
    public float shootSpeed = 3;

    Animator animator;

    public Transform firePoint;
    List<Transform> firePoints = null;
    int firePointIdx = 0;
    public GameObject shootPrefab;

    public UnityEvent shootEvent;

    [Range(0, 180)] public float dAngleShoot = 0; // if lookTarget
    public AnimationCurve dAngleShootCurve;

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if(dAngleShoot > 0)
        {
            Gizmos.color = Color.yellow;
            Tool.GizmoDrawWireCone(firePoint.position, firePoint.forward, view, dAngleShoot);
        }
    }



    void Awake()
    {
        animator = GetComponent<Animator>();

        if(firePoint.childCount > 0)
        {
            firePoints = new List<Transform>();
            foreach (Transform child in firePoint)
                firePoints.Add(child);
            firePoint = firePoints[0];
        }

        TargetEnterFOV.AddListener(() => { animator.SetBool("shooting", true); });
        TargetExitFOV .AddListener(() => { animator.SetBool("shooting", false); });
    }

    private void OnEnable()
    {
        animator.SetFloat("shootingSpeed", shootSpeed);
    }

    protected virtual void Shoot()
    {
        if (shootPrefab)
        {
            GameObject shoot = QueueManager.Instantiate(shootPrefab.gameObject, firePoint.position, firePoint.rotation, false);
            shoot.GetComponent<ForceField>()?.SetSizeMax(view * 2);
            shoot.GetComponent<ProjectileTarget>()?.SetTarget(target);
            shoot.GetComponent<MultiProjectile>()?.SetTarget(target);
            shoot.GetComponent<Bolt>()?.SetTarget(target.position);

            if (dAngleShoot > 0)
            {
                shoot.transform.Rotate(Vector3.forward * Tool.Rand(360f));
                shoot.transform.Rotate(Vector3.right * dAngleShootCurve.Evaluate(Tool.Rand(1f)) * dAngleShoot);
            }


            shoot.SetActive(true);
        }

        if (firePoints != null)
        {
            firePointIdx++;
            firePointIdx %= firePoints.Count;
            firePoint = firePoints[firePointIdx];
        }

        shootEvent.Invoke();
    }


    public void SetShootSpeed(float speed)
    {
        shootSpeed = speed;
        animator.SetFloat("shootingSpeed", shootSpeed);
    }
}
