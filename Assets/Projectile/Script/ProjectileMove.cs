using UnityEngine;


public class ProjectileMove : Projectile
{
    public float life = 5;
    protected float lifeCrt = 0;

    public float speed = 10;
    public int moveResolution = 1;

    protected virtual void Awake()
    {
        speed /= moveResolution;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        lifeCrt = 0;
    }

    protected virtual void Update()
    {
        if (moveResolution == 1) Move(speed * Time.deltaTime);
        else for (int i = 0; i < moveResolution && !Move(speed * Time.deltaTime); i++) { }

        lifeCrt += Time.deltaTime;
        if (lifeCrt > life) Desactive();
    }

    protected virtual bool Move(float dist)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, dist, layer))
        {
            transform.position = hit.point;

            Tool.SearchComponent<Enemy>(hit.transform)?.TakeDamage(damage);

            if (impactPrefab)
                QueueManager.Instantiate(impactPrefab.gameObject,
                                         hit.point,
                                         Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal)));

            Desactive();
            return true;
        }

        transform.Translate(Vector3.forward * dist);
        return false;
    }

    protected virtual void Desactive() => QueueManager.Desactive(gameObject);

    public void SetSpeed(float speed) => this.speed = speed / moveResolution;

}
