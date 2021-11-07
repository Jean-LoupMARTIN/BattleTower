using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    static public List<Enemy> list = new List<Enemy>();


    public float life = 100;


    private void Awake()
    {
        list.Add(this);
    }

    private void OnDestroy()
    {
        list.Remove(this);
    }

    static public Enemy GetClosest(Vector3 point)
    {
        Enemy closest = null;
        float distToClosest = 1000;

        foreach (Enemy enemy in list)
        {
            if (!enemy.gameObject.activeInHierarchy)
                continue;

            float distToEnemy = Tool.Dist(point, enemy);
            if (distToEnemy < distToClosest)
            {
                distToClosest = distToEnemy;
                closest = enemy;
            }
        }
        return closest;
    }


    public void TakeDamage(float dmg)
    {
        life -= dmg;
        if (life <= 0) Destroy(gameObject);
    }
}
