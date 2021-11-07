using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Particle shootPrefab, impactPrefab;
    public enum ImpactDirection { Reflect, Forward, Back }
    public ImpactDirection impactDirection = ImpactDirection.Reflect;
    public float damage = 10;
    public LayerMask layer;// = (1 << 9) | (1 << 10) | (1 << 11);

    protected virtual void OnEnable()
    {
        if (shootPrefab)
            QueueManager.Instantiate(shootPrefab.gameObject, transform.position, transform.rotation);
    }

    protected void InstantiateImpact(Vector3 hitPoint, Vector3 hitForward, Vector3 hitNormal)
    {
        Vector3 dir = transform.forward;
        if      (impactDirection == ImpactDirection.Reflect)    dir = Vector3.Reflect(transform.forward, hitNormal);
        else if (impactDirection == ImpactDirection.Forward)    dir = hitForward;
        else if (impactDirection == ImpactDirection.Back)       dir =-hitForward;
        QueueManager.Instantiate(impactPrefab.gameObject, hitPoint, Quaternion.LookRotation(dir));
    }
}
