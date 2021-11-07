
using UnityEngine;

public class ProjectileTarget : ProjectileTrail
{
    public float rotSpeed = 5f;
    public Transform target;


    protected override void Move()
    {
        if (target)
        {
            Quaternion rotTarget = Quaternion.LookRotation(Tool.Dir(transform, target));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * rotSpeed);
        }
        base.Move();
    }


    public void SetTarget(Transform target) => this.target = target;
}
