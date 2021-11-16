using UnityEngine;

public class MultiProjectile : MonoBehaviour
{
    public ProjectileTarget projoPrefab;
    public int nbProjo = 3;
    public float angle = 90;
    public Particle ShootParticlePrefab;
    public Transform target;

    public float speed = 10f;
    [Range(0, 1)]
    public float dSpeed = 0.1f;
    public float rotSpeed = 5f;


    protected virtual void OnEnable()
    {
        if (projoPrefab)
        {
            for (int i = 0; i < nbProjo; i++)
            {
                ProjectileTarget bullet = QueueManager.Instantiate(projoPrefab.gameObject, transform.position, transform.rotation).GetComponent<ProjectileTarget>();
                bullet.transform.Rotate(Random.Range(-angle / 2, angle / 2), Random.Range(-angle / 2, angle / 2), 0);
                bullet.target = target;
                bullet.SetSpeed(speed * (1 + Random.Range(-dSpeed, dSpeed)));
                bullet.SetRotSpeed(rotSpeed);
            }
        }

        if (ShootParticlePrefab)
            QueueManager.Instantiate(ShootParticlePrefab.gameObject, transform.position, transform.rotation);

        QueueManager.Desactive(gameObject);
        //Invoke("Desactive", 0.05f);
    }


    public void SetTarget(Transform target) => this.target = target;

    //void Desactive() => QueueManager.Desactive(gameObject);
}
