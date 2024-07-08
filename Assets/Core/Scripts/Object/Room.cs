using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    [field: SerializeField] private List<GameObject> spawnableBlocks;
    [field: SerializeField] private List<RoomStage> stages = new List<RoomStage>();
    [field: SerializeField] private List<GameObject> bridges = new List<GameObject>();

    public int RemainEnemy;

    private bool isEntered = false;
    private bool isClear = false;
    private int stageIndex = 0;
    private Vector2Int blockCoords;

    private string bridgeMeta;
    private BlockType type;

    private void Start()
    {
        string[] splt = name.Split('_');
        bridgeMeta = splt[0];
        blockCoords = new Vector2Int(int.Parse(splt[2]), int.Parse(splt[3]));

        type = (BlockType)int.Parse(splt[1]);

        if (type == BlockType.Start)
        {
            ShowBridges(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!LevelManager.Instance.LayerData.IsPlayerLayer(other.gameObject.layer))
        {
            return;
        }

        if (!isEntered)
            LevelManager.Instance.SetTargetRoom(this);

        if (!isEntered)
            Spawns(stageIndex);

        isEntered = true;

        Vector2 delta = -blockCoords + LevelManager.Instance.PrevBlock;

        if (delta.x == 1)
            LevelManager.Instance.Generator.MoveRight();
        else if (delta.x == -1)
            LevelManager.Instance.Generator.MoveLeft();
        else if (delta.y == 1)
            LevelManager.Instance.Generator.MoveTop();
        else if (delta.y == -1)
            LevelManager.Instance.Generator.MoveBottom();

        LevelManager.Instance.PrevBlock = blockCoords;
    }

    public void SetBridges(List<GameObject> bridges)
    {
        this.bridges = bridges;
    }

    public void SetClear(bool clear)
    {
        isClear = clear;
    }

    public bool NextStage()
    {
        stageIndex += 1;

        if (stageIndex < stages.Count)
        {
            Spawns(stageIndex);

        }
        else
        {
            if (type == BlockType.Boss)
                return true;

            isClear = true;
            ShowBridges();
        }

        return false;

    }


    private void Spawns(int stageIndex)
    {
        if (stages == null || stages.Count == 0)
            return;

        RemainEnemy = 0;

        RoomStage stage = stages[stageIndex];

        foreach (EnemyBucket bucket in stage.EnemyBuckets)
        {
            int[] spawnIdxs = GenerateRandomArray(bucket.Amount, 0, spawnableBlocks.Count);

            RemainEnemy += bucket.Amount;

            for (int i = 0; i < bucket.Amount; ++i)
            {
                EnemyHandler.Instance.Spawn(bucket.EnemyPrefab.ID, spawnableBlocks[spawnIdxs[i]].transform.position);
            }
        }

    }

    private int[] GenerateRandomArray(int length, int minValue, int maxValue)
    {
        int[] array = new int[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = UnityEngine.Random.Range(minValue, maxValue);
        }
        return array;
    }


    private void ShowBridges(bool floatAnimation = true)
    {
        List<int> activeBrideges = new List<int>();

        for (int i = bridgeMeta.Length - 1; i >= 0; --i)
        {
            if (bridgeMeta[i] == '1')
            {
                activeBrideges.Add(3 - i);
                bridges[3 - i].SetActive(true);

                if (!floatAnimation)
                {
                    Vector3 pos = bridges[3 - i].transform.localPosition;
                    pos.y = 1.6f;
                    bridges[3 - i].transform.localPosition = pos;
                }
            }

            
        }

        if (floatAnimation)
            StartCoroutine(FloatBridge(activeBrideges));
        else
        {
            LevelManager.Instance.UpdateNavMeshInBounds();
        }

    }

    private IEnumerator FloatBridge(List<int> activeBridges)
    {
        float lerp = 0;

        while (lerp < 1)
        {
            foreach (int i in activeBridges)
            {

                Vector3 pos = bridges[i].transform.localPosition;
                pos.y = Mathf.Lerp(pos.y, 1.6f, lerp);

                lerp += Time.deltaTime * 0.5f;

                bridges[i].transform.localPosition = pos;
            }

            yield return null;
        }

        LevelManager.Instance.UpdateNavMeshInBounds();

    }
}


[Serializable]
struct EnemyBucket
{
    public PoolingEntity EnemyPrefab;
    public int Amount;
    public bool Boss;
}

[Serializable]
struct RoomStage
{
    public List<EnemyBucket> EnemyBuckets;
}