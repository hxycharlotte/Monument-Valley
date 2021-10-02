using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 记：协程引起的隐性BUG
 * 外部不能即时禁用物体，在update里调用可能导致多次回收
 * 故弃用协程
 */
public class GameObjectPool : MonoSingleton<GameObjectPool>
{
    private Dictionary<string, Queue<GameObject>> cache;

    private void Awake()
    {
        cache = new Dictionary<string, Queue<GameObject>>();
    }

    public GameObject Obtain(string key, GameObject origin, Vector3 position, Quaternion quaternion)
    {
        Queue<GameObject> queue = GetQueue(key);

        if (queue.Count == 0)
        {
            GameObject cloneGO = Instantiate(origin);
            queue.Enqueue(cloneGO);
        }

        GameObject go = GetGameObject(queue, position, quaternion);

        return go;
    }

    public void ObjectCollect(string key, GameObject go)
    {
        //可能在release后调用，保证鲁棒性
        Queue<GameObject> queue = GetQueue(key);
        go.SetActive(false);
        queue.Enqueue(go);
    }

    public void Release(string key)
    {
        if (!cache.ContainsKey(key))
            return;

        Queue<GameObject> queue = cache[key];

        while (queue.Count > 0)
        {
            Destroy(queue.Dequeue());
        }

        cache.Remove(key);
    }

    public void ReleaseAll()
    {
        foreach (string key in new List<string>(cache.Keys))
        {
            Release(key);
        }
    }

    private Queue<GameObject> GetQueue(string key)
    {
        if (!cache.ContainsKey(key))
            cache.Add(key, new Queue<GameObject>());

        return cache[key];
    }

    private GameObject GetGameObject(Queue<GameObject> queue, Vector3 position, Quaternion quaternion)
    {
        GameObject go = queue.Dequeue();
        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = quaternion;
        return go;
    }
}