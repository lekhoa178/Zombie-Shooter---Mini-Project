using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PoolingManager>();

            return instance;
        }
    }
    private static PoolingManager instance;

    [field: SerializeField] private List<PoolingEntityContainer> EntityPool = new List<PoolingEntityContainer>();

    [field: SerializeField] public Dictionary<int, Pool> Pools { get; private set; } = new Dictionary<int, Pool>();

    private void Awake()
    {
        foreach (PoolingEntityContainer container in EntityPool)
        {
            Pools.Add(container.Entity.ID, new Pool(container.Entity.Prefab, container.Amount, container.Parent));
        }
    }

    public Pool GetPool(int id)
    {
        return Pools[id];
    }
}

[Serializable]
public struct PoolingEntityContainer
{
    [field: SerializeField] public PoolingEntity Entity { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
    [field: SerializeField] public GameObject Parent { get; private set; }
}