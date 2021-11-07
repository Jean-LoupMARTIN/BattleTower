using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstante : Projectile
{
    protected override void OnEnable()
    {
        base.OnEnable();

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100, layer))
        {
            Tool.SearchComponent<Enemy>(hit.transform)?.TakeDamage(damage);

            if (impactPrefab)
                InstantiateImpact(hit.point, transform.forward, hit.normal);
        }

        QueueManager.Desactive(gameObject);
    }
}
