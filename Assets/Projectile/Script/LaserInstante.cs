using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LaserInstante : MonoBehaviour
{
    public float dps = 10;
    public ParticleSystem impact;
    public LayerMask layer;

    LineRenderer line;


    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        Update();
    }

    private void Update()
    {
        line.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100, layer))
        {
            line.SetPosition(1, hit.point);
            impact.gameObject.SetActive(true);
            impact.transform.position = hit.point;
            impact.transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal));
            impact.transform.position += impact.transform.forward * 0.01f;
            Tool.SearchComponent<Enemy>(hit.transform)?.TakeDamage(dps * Time.deltaTime);
        }
        else {
            line.SetPosition(1, transform.position + transform.forward * 100);
            impact.gameObject.SetActive(false);
        } 
    }
}
