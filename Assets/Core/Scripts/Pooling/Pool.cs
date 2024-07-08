using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pool
{
    [field: SerializeField] public Queue<GameObject> ObjectPool;
    [field: SerializeField] public List<GameObject> ActivePool;

    private GameObject objectPrefab;
    private GameObject parent;

    public Pool(GameObject objectPrefab, int amount, GameObject parent)
    {
        this.objectPrefab = objectPrefab;
        this.parent = parent;

        ObjectPool = new Queue<GameObject>();
        ActivePool = new List<GameObject>();

        for (int k = 0; k < amount; ++k)
        {
            Create();
        }
    }

    public GameObject Get()
    {
        if (ObjectPool.Count == 0)
        {
            Create();
        }

        GameObject go = ObjectPool.Dequeue();
        ActivePool.Add(go);

        return go;
    }

    public void Return(GameObject gameObject)
    {
        int found = ActivePool.IndexOf(gameObject);

        if (found == -1)
        {
            return;
        }

        GameObject go = ActivePool[found];
        go.SetActive(false);
        ActivePool.RemoveAt(found);
        ObjectPool.Enqueue(go);
    }

    private void Create()
    {
        GameObject go = UnityEngine.Object.Instantiate(objectPrefab,
            parent != null ? parent.transform : PoolingManager.Instance.transform);
        go.name = "!" + go.name;
        go.SetActive(false);
        ObjectPool.Enqueue(go);
    }
}
