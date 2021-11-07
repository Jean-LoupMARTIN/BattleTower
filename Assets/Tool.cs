using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public static class Tool
{
    // GAMEOBJECT
    static public void DelayAction(Action action, float delay) => GameObject.FindObjectOfType<MonoBehaviour>().StartCoroutine(DelayActionCoroutine(action, delay));
    static IEnumerator DelayActionCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    static public void ActionWaitEndFrame(Action action, int nbFrame = 1) => GameObject.FindObjectOfType<MonoBehaviour>().StartCoroutine(ActionWaitEndFrameCoroutine(action, nbFrame));
    static IEnumerator ActionWaitEndFrameCoroutine(Action action, int nbFrame)
    {
        for(int i = 0; i < nbFrame; i++)
            yield return new WaitForEndOfFrame();
        action.Invoke();
    }


    public static T SearchComponent<T>(Transform trans)
    {
        if (trans == null) return default(T);

        T compo = trans.GetComponent<T>();
        if (compo != null) return compo;
        return SearchComponent<T>(trans.parent);
    }


    //public static void SetAlphaGradient(TrailRenderer trail, float a)
    //{
    //    GradientAlphaKey[] keys = new GradientAlphaKey[trail.colorGradient.alphaKeys.Length];

    //    for (int i = 0; i < keys.Length; i++)
    //        keys[i] = new GradientAlphaKey(a, trail.colorGradient.alphaKeys[i].time);
    //    Gradient gradient = trail.colorGradient;
    //    gradient.SetKeys(trail.colorGradient.colorKeys, keys);
    //    trail.colorGradient = gradient;
    //}


    // SHAKE
    public static void ShakeSphere(Vector3 pos, float radius)
    {
        if (!CameraShaker.Instance)
            return;

        float dist = Dist(pos, CameraShaker.Instance.transform);
        if (dist < radius)
        {
            float force = (radius - dist) * 0.3f;
            CameraShaker.Instance.ShakeOnce(force, force, 0, 0.5f);
        }
    }


    //GIZMO
    public static void GizmoDrawCircle(Vector3 pos, Vector3 forward, float radius, int resolution = 30)
    {
        Quaternion rot = Quaternion.LookRotation(forward);
        float drot = 360 / resolution;

        for (int i = 0; i < resolution; i++)
        {
            Vector3 from = pos + rot * Vector3.up * radius;
            rot *= Quaternion.Euler(Vector3.forward * drot);
            Vector3 to = pos + rot * Vector3.up * radius;

            Gizmos.DrawLine(from, to);
        }
    }

    public static void GizmoDrawWireCone(Vector3 pos, Vector3 forward, float length, float angle)
    {
        angle /= 2;
        Quaternion rot;

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.right * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.left * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.up * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.down * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        angle = Mathf.Deg2Rad * angle;
        float adj = Mathf.Cos(angle) * length;
        float opp = Mathf.Sin(angle) * length;

        GizmoDrawCircle(pos + forward * adj, forward, opp);
    }


    // MATH
    public static float Progress(float t, float max) => Mathf.Clamp01(t / max);
    public static bool Proba(float proba)   => Rand(1f)   < proba;
    public static bool Percent(float proba) => Rand(100f) < proba;



    // LIST
    public static int   Rand(int   max) { return UnityEngine.Random.Range(0, max); }
    public static float Rand(float max) { return UnityEngine.Random.Range(0, max); }

    public static T Rand<T>(T[]     list) { return list[Rand(list.Length)]; }
    public static T Rand<T>(List<T> list) { return list[Rand(list.Count)]; }

    public static T Last<T>(T[] list) { return list[list.Length - 1]; }
    public static T Last<T>(List<T> list) { return list[list.Count - 1]; }




    // DIRECTION 
    public static float Dist(MonoBehaviour a, MonoBehaviour b) { return Dist(a.transform, b.transform); }
    public static float Dist(Vector3       a, MonoBehaviour b) { return Dist(a,           b.transform); }
    public static float Dist(MonoBehaviour a, Vector3       b) { return Dist(a.transform, b          ); }
    public static float Dist(Transform     a, MonoBehaviour b) { return Dist(a,           b.transform); }
    public static float Dist(MonoBehaviour a, Transform     b) { return Dist(a.transform, b          ); }
    public static float Dist(Transform     a, Transform     b) { return Dist(a.position,  b.position ); }
    public static float Dist(Vector3       a, Transform     b) { return Dist(a,           b.position ); }
    public static float Dist(Transform     a, Vector3       b) { return Dist(a.position,  b          ); }
    public static float Dist(Vector3       a, Vector3       b) { return (a - b).magnitude; }

    public static Vector3 Dir(MonoBehaviour a, MonoBehaviour b, bool normalized = false) { return Dir(a.transform, b.transform, normalized); }
    public static Vector3 Dir(Vector3       a, MonoBehaviour b, bool normalized = false) { return Dir(a,           b.transform, normalized); }
    public static Vector3 Dir(MonoBehaviour a, Vector3       b, bool normalized = false) { return Dir(a.transform, b,           normalized); }
    public static Vector3 Dir(Transform     a, MonoBehaviour b, bool normalized = false) { return Dir(a,           b.transform, normalized); }
    public static Vector3 Dir(MonoBehaviour a, Transform     b, bool normalized = false) { return Dir(a.transform, b,           normalized); }
    public static Vector3 Dir(Transform     a, Transform     b, bool normalized = false) { return Dir(a.position,  b.position,  normalized); }
    public static Vector3 Dir(Vector3       a, Transform     b, bool normalized = false) { return Dir(a,           b.position,  normalized); }
    public static Vector3 Dir(Transform     a, Vector3       b, bool normalized = false) { return Dir(a.position,  b,           normalized); }
    public static Vector3 Dir(Vector3       a, Vector3       b, bool normalized = false) {
        if (normalized) return (b - a).normalized;
        return b - a;
    }
}
