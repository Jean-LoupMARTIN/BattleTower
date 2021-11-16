using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class ProjectileTrail : ProjectileMove
{
    [HideInInspector]
    public TrailRenderer trail;
    bool moving = true;
    public GameObject body;

    protected override void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        base.Awake();
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        trail.Clear();
        moving = true;
        if (body != null) body.SetActive(true);
    }

    protected override void Desactive()
    {
        moving = false;
        if (body != null) body.SetActive(false);
        Invoke("DesactiveBase", trail.time);
    }

    void DesactiveBase() => base.Desactive();

    protected override void Update()
    {
        if (!moving) return;
        base.Update();
    }

    protected override bool Move(float dist)
    {
        trail.AddPosition(transform.position);
        return base.Move(dist);
    }
}
