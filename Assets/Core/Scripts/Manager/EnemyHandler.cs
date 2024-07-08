using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public static EnemyHandler Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<EnemyHandler>();

            return instance;
        }
    }
    private static EnemyHandler instance;

    [field: SerializeField] private PoolingEntity dangerPrefab;
    [field: SerializeField] private float timeToSpawn = 1.5f;

    private Queue<EnemyEntity> EnemyEntities = new Queue<EnemyEntity>();

    public void Spawn(int enemyId, Vector3 pos)
    {
        GameObject go = PoolingManager.Instance.GetPool(dangerPrefab.ID).Get();
        pos.y = 0.23f;
        go.transform.position = pos;
        go.SetActive(true);

        EnemyEntities.Enqueue(new EnemyEntity(go, Time.time, enemyId));
    }

    private void Update()
    {
        for (int i = 0; i <  EnemyEntities.Count; i++)
        {
            if (Time.time > EnemyEntities.Peek().startTime + timeToSpawn)
            {
                EnemyEntity ee = EnemyEntities.Dequeue();

                PoolingManager.Instance.GetPool(dangerPrefab.ID).Return(ee.dangerPing);
                GameObject enemy = PoolingManager.Instance.GetPool(ee.enemyId).Get();
                enemy.transform.position = ee.dangerPing.transform.position;
                enemy.name = ee.enemyId + "_Enemy";
                enemy.SetActive(true);
            }
            else
                break;
        }
    }
}

struct EnemyEntity
{
    public GameObject dangerPing;
    public float startTime;
    public int enemyId;

    public EnemyEntity(GameObject dangerPing, float startTime, int enemyId)
    {
        this.dangerPing = dangerPing;
        this.startTime = startTime;
        this.enemyId = enemyId;
    }
}