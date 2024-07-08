using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AnticipationHandler : MonoBehaviour
{
    public static AnticipationHandler Instance;

    [field: SerializeField] private PoolingEntity CircularPrefab;

    private Pool circularPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        circularPool = PoolingManager.Instance.GetPool(CircularPrefab.ID);
    }

    public GameObject CircularAnticipation(Vector3 pos, float radius)
    {
        GameObject go = circularPool.Get();
        go.transform.position = pos;
        go.transform.localScale = radius * Vector3.one;
        go.SetActive(true);

        return go;
    }

    public void UpdatePosition(GameObject ant, Vector3 pos)
    {
        ant.transform.position = pos;
    }

    public void ReturnCircularAnticipation(GameObject go)
    {
        circularPool.Return(go);
    }
}