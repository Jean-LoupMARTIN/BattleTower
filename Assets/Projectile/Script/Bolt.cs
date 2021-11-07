

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Bolt : Projectile
{
    public LineRenderer line, subLine;
    public float lineWidth = 0.1f;

    public float time = 0.5f;
    float timeCrt = 0;

    public float distPoint = 0.1f;
    public float dPoint = 0.03f;





    public void SetTarget(Vector3 target)
    {
        timeCrt = 0;
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> positionsSub = new List<Vector3>();

        Vector3 A = transform.position;
        Vector3 B;
        Vector3 C = target;
        float distAC = Tool.Dist(A, C);
        float ABC = Vector3.Angle(Tool.Dir(A, C), transform.forward) * Mathf.Deg2Rad;
        float adjacent = distAC / 2;
        float hypothenus = adjacent / Mathf.Cos(ABC); // Cos(ABC) = adjacent / hypothenus
        B = transform.position + transform.forward * hypothenus;

        int nbPoint = (int)(distAC / distPoint);
        Vector3 posMem = Vector3.zero;

        for(int i = 0; i < nbPoint; i++)
        {
            float t = Tool.Progress(i, nbPoint-1);
            Vector3 pos = (1 - t) * Vector3.Lerp(A, B, t) + t * Vector3.Lerp(B, C, t);
            if (positions.Count > 0 && Physics.Raycast(posMem, Tool.Dir(posMem, pos), out RaycastHit hit, Tool.Dist(posMem, pos), layer))
            {
                positions.Add(hit.point);
                positionsSub.Add(hit.point);
                Tool.SearchComponent<Enemy>(hit.transform)?.TakeDamage(damage);

                if (impactPrefab)
                    InstantiateImpact(hit.point, Tool.Dir(posMem, pos), hit.normal);

                break;
            }

            posMem = pos;
            Vector3 posSub = pos;

            if (positions.Count > 0)
            {
                pos    += dPoint * new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                posSub += dPoint * new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            }
                
            positions.Add(pos);
            positionsSub.Add(posSub);
        }

        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());

        subLine.positionCount = positionsSub.Count;
        subLine?.SetPositions(positionsSub.ToArray());
    }

    void Update()
    {
        timeCrt += Time.deltaTime;
        line.startWidth = lineWidth * (1 - timeCrt / time);
        if (subLine) subLine.startWidth = lineWidth / 4 * Mathf.Max(1 - timeCrt / (time*0.75f), 0);
        if (timeCrt > time) QueueManager.Desactive(gameObject);
    }
}
