
using UnityEngine;

public class ProjectileTarget : ProjectileTrail
{
    public float rotSpeed = 5f;
    public Transform target;

    protected override void Awake()
    {
        base.Awake();
        rotSpeed /= moveResolution;
    }

    protected override bool Move(float dist)
    {
        if (target)
        {
            Quaternion rotTarget = Quaternion.LookRotation(Tool.Dir(transform, target));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * rotSpeed);
        }

        return base.Move(dist);
    }


    public void SetTarget(Transform target) => this.target = target;

    public void SetRotSpeed(float rotSpeed) => this.rotSpeed = rotSpeed / moveResolution;
}
