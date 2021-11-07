
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceField : MonoBehaviour
{
    public float life = 1;
    float lifeCrt = 0;
    public float damage = 10, sizeMax = 10;
    public AnimationCurve sizeCurve;
    public AudioSource shootSource;
    Rigidbody rb;
    HashSet<Enemy> enemiesHit = new HashSet<Enemy>();
    public Particle impactParticlePrefab;
    public float shakeForce = 3;

    MeshRenderer mr;
    float _textureScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        _textureScale = mr.material.GetFloat("_textureScale");
    }

    protected virtual void OnEnable()
    {
        lifeCrt = 0;
        shootSource.Play();
        enemiesHit.Clear();

        SetSize(0);
        rb.MovePosition(transform.position);
    }

    private void Update()
    {
        lifeCrt += Time.deltaTime;
        if (lifeCrt > life) QueueManager.Desactive(gameObject);
        else SetSize(sizeMax * sizeCurve.Evaluate(Tool.Progress(lifeCrt, life)));
    }

    public void SetSizeMax(float sizeMax) => this.sizeMax = sizeMax;


    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPoint = collision.contacts[0].point;

        Enemy e = Tool.SearchComponent<Enemy>(collision.transform);
        if(e && !enemiesHit.Contains(e))
        {
            e.TakeDamage(damage);
            enemiesHit.Add(e);
            if (impactParticlePrefab)
                QueueManager.Instantiate(impactParticlePrefab.gameObject, hitPoint, Quaternion.LookRotation(Tool.Dir(hitPoint, transform, true)));
        }

        if (collision.gameObject.name == "Cam Container")
            Tool.ShakeSphere(hitPoint, shakeForce);
    }

    void SetSize(float size)
    {
        rb.MovePosition(transform.position);
        transform.localScale = Vector3.one * size;
        rb.MovePosition(transform.position);

        mr.material.SetFloat("_textureScale", _textureScale * size);
    }
}
