using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class ProjectileTrail : ProjectileMove
{
    TrailRenderer trail;
    float timeMem;
    bool moving = true;
    public GameObject body;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        timeMem = trail.time;
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        trail.time = 0;
        Tool.ActionWaitEndFrame(() => { trail.time = timeMem; }, 2);
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
}
