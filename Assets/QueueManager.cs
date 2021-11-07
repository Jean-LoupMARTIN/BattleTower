using System;
using System.Collections.Generic;
using UnityEngine;

public static class QueueManager
{
    static Dictionary<string, Queue<GameObject>> dict = new Dictionary<string, Queue<GameObject>>();



    public static GameObject Instantiate(GameObject prefab, Vector3 pos, Quaternion rot, bool active = true)
    {
        GameObject inst;

        if (dict.ContainsKey(prefab.name) && dict[prefab.name].Count > 0)
        {
            inst = dict[prefab.name].Dequeue();
            inst.transform.position = pos;
            inst.transform.rotation = rot;
        }

        else {
            bool prefabActive = prefab.activeSelf;
            prefab.SetActive(false);
            inst = GameObject.Instantiate(prefab, pos, rot);
            prefab.SetActive(prefabActive);
            inst.name = prefab.name;
        }

        inst.SetActive(active);
        return inst;
    }


    static public void Desactive(GameObject go)
    {
        if (!dict.ContainsKey(go.name))
            dict[go.name] = new Queue<GameObject>();

        go.SetActive(false);
        dict[go.name].Enqueue(go);
    }
}
