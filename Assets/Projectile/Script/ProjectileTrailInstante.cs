using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(LineRenderer))]
public class ProjectileTrailInstante : Projectile
{
    LineRenderer line;
    public float time = 1;
    float timeCrt;
    public float lineWidth = 0.1f;
    public AnimationCurve widthCurve;


    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        SetLineWidth(lineWidth);

        line.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100, layer))
        {
            line.SetPosition(1, hit.point);

            Tool.SearchComponent<Enemy>(hit.transform)?.TakeDamage(damage);

            if (impactPrefab)
                InstantiateImpact(hit.point, transform.forward, hit.normal);
        }
        else line.SetPosition(1, transform.position + transform.forward * 100);

        timeCrt = 0;

    }



    private void Update()
    {
        timeCrt += Time.deltaTime;

        if (timeCrt >= time) QueueManager.Desactive(gameObject);

        else SetLineWidth(widthCurve.Evaluate(timeCrt / time) * lineWidth);
    }

    void SetLineWidth(float width)
    {
        line.startWidth = width;
        line.endWidth = line.startWidth;
    }
}
