using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    List<Transform> points;
    Transform lastPoint;

    Transform pivot;

    public float t180 = 1;
    public float tWait = 1;

    public AnimationCurve rotCurve;



    private void Awake()
    {
        pivot = transform.Find("Pivot");

        points = new List<Transform>();
        foreach (Transform point in transform.Find("Points"))
            if (point.gameObject.activeSelf) points.Add(point);

        lastPoint = Tool.Rand(points);
        pivot.forward = Tool.Dir(pivot, lastPoint);

        Invoke("NewPoint", tWait);
    }


    void NewPoint()
    {
        Transform newPoint = lastPoint;
        while (newPoint == lastPoint)
            newPoint = Tool.Rand(points);
        lastPoint = newPoint;

        Vector3 dirToPoint = Tool.Dir(pivot, newPoint);
        Quaternion rotToPoint = Quaternion.LookRotation(dirToPoint);
        float angle = Vector3.Angle(pivot.forward, dirToPoint);
        StartCoroutine(SmoothRot(rotToPoint, t180 * Mathf.Sqrt(angle / 180)));
    }



    


    IEnumerator SmoothRot(Quaternion rotEnd, float t)
    {
        Quaternion rotStart = pivot.rotation;

        float tlast = Time.time, tnew, dt;
        float tcrt = 0, progress = 0;

        while(tcrt < t)
        {
            tnew = Time.time;
            dt = tnew - tlast;
            tlast = tnew;

            tcrt += dt;
            progress = Tool.Progress(tcrt, t);
            pivot.rotation = Quaternion.Lerp(rotStart, rotEnd, rotCurve.Evaluate(progress));

            yield return new WaitForEndOfFrame();
        }

        pivot.rotation = rotEnd;
        Invoke("NewPoint", tWait);
    }





}
