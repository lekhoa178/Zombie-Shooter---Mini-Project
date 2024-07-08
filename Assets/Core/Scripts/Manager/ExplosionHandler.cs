using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public static ExplosionHandler Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ExplosionHandler>();

            return instance;
        }
    }
    private static ExplosionHandler instance;

    private Queue<ExplosionEntity> explosionGO = new Queue<ExplosionEntity>();

    public void Initialize(int id, Vector3 pos, float radius, float damage, int sustain)
    {
        GameObject go = PoolingManager.Instance.GetPool(id).Get();
        go.transform.position = pos;
        go.name = damage.ToString() + "_" + sustain;

        radius = Mathf.Max(0.2f, radius);
        go.transform.localScale = Vector3.one * radius;
        go.SetActive(true);
        explosionGO.Enqueue(new ExplosionEntity(go, id, Time.time));
    }

    private void Update()
    {
        for (int i = explosionGO.Count - 1; i >= 0; i--)
        {
            if (Time.time > explosionGO.Peek().startTime + 5f)
            {
                ExplosionEntity ent = explosionGO.Dequeue();
                ent.go.SetActive(false);
                PoolingManager.Instance.GetPool(ent.id).Return(ent.go);
            }
            else
                break;
        }
    }
}

struct ExplosionEntity
{
    public GameObject go;
    public int id;
    public float startTime;

    public ExplosionEntity(GameObject go, int id, float startTime)
    {
        this.go = go;
        this.id = id;
        this.startTime = startTime;
    }
}