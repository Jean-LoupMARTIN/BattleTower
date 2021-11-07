using UnityEngine;


public class ProjectileMove : Projectile
{
    public float life = 5;
    protected float lifeCrt = 0;

    public float speed = 10;


    protected override void OnEnable()
    {
        base.OnEnable();
        lifeCrt = 0;
    }

    protected virtual void Update()
    {
        Move();

        lifeCrt += Time.deltaTime;
        if (lifeCrt > life) Desactive();
    }

    protected virtual void Move()
    {
        float move = speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, move, layer))
        {
            transform.position = hit.point;

            Tool.SearchComponent<Enemy>(hit.transform)?.TakeDamage(damage);

            if (impactPrefab)
                QueueManager.Instantiate(impactPrefab.gameObject,
                                         hit.point,
                                         Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal)));

            Desactive();
        }
        else transform.Translate(Vector3.forward * move);
    }

    protected virtual void Desactive() => QueueManager.Desactive(gameObject);
}
