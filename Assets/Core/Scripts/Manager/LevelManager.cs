using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [field: Header("UI Page")]
    [field: SerializeField] public Page BasePage { get; private set; }

    [field: SerializeField] public GlobalLayerData LayerData { get; private set; }
    [field: SerializeField] public int NumberOfZones { get; private set; } = 2;

    [field: SerializeField] private GameObject parentPrefab;
    [field: SerializeField] private List<RoomEntity> roomEntities;
    [field: SerializeField] private Vector2 offset;
    public MapGenerator Generator { get; private set; }

    public Vector2Int PrevBlock { get; set; }

    public void SetTargetRoom(Room room) => targetRoom = room;

    public float GetPlayerHealth(float maxHealth) {
        if (currentPlayerHealth == -1)
        {
            HealthController.Instance.Initialize(maxHealth);
            return maxHealth;
        }
        else
        {
            HealthController.Instance.UpdateHealth(currentPlayerHealth, maxHealth);
            return currentPlayerHealth;
        }
    }

    public float RegisterPlayerHealth(float health) => currentPlayerHealth = health;


    private Room targetRoom;
    private Transform parent;
    private NavMeshSurface surface;

    private Dictionary<Vector2Int, bool> visited = new Dictionary<Vector2Int, bool>();
    private int currentZone = 0;
    private float currentPlayerHealth = -1;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        Generator = GetComponent<MapGenerator>();
    }

    public void ResetState()
    {
        PrevBlock = Vector2Int.zero;
        targetRoom = null;
        currentZone = 0;
        currentPlayerHealth = -1;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home")
        {
            return;
        }

        parent = Instantiate(parentPrefab).transform;
        surface = parent.GetComponent<NavMeshSurface>();
        visited = new Dictionary<Vector2Int, bool>();

        UIManager.Instance.PushPage(BasePage);

        GenerateRoom();
    }

    public void NextZone()
    {
        currentZone++;
        if (currentZone < NumberOfZones)
        {
            GameManager.Instance.ReloadScene();
        }
    }

    public void UpdateNavMeshInBounds()
    {
        surface.BuildNavMesh();
    }

    private void GenerateRoom()
    {
        Generator.Initialize(currentZone == NumberOfZones - 1);

        foreach (Vector2Int cords in Generator.GetPath())
        {
            visited[cords] = false;
        }

        foreach (Vector2Int cords in Generator.GetPath())
        {
            visited[cords] = true;

            MapBlock block = Generator.GetBlock(cords.x, cords.y);
            BlockDirection dir = block.direction;
            int blockDir = (int)dir;

            int bridgeDir = 0;

            Vector2Int cordsT = new Vector2Int(cords.x, cords.y + 1);
            if (visited.ContainsKey(cordsT) && (blockDir & 1 << 1) != 0 && !visited[cordsT])
            {
                bridgeDir |= 1 << 0;
            }

            Vector2Int cordsR = new Vector2Int(cords.x + 1, cords.y);
            if (visited.ContainsKey(cordsR) && (blockDir & 1 << 2) != 0 && !visited[cordsR])
            {
                bridgeDir |= 1 << 1;
            }

            Vector2Int cordsB = new Vector2Int(cords.x, cords.y - 1);
            if (visited.ContainsKey(cordsB) && (blockDir & 1 << 3) != 0 && !visited[cordsB])
            {
                bridgeDir |= 1 << 2;
            }

            Vector2Int cordsL = new Vector2Int(cords.x - 1, cords.y);
            if (visited.ContainsKey(cordsL) && (blockDir & 1 << 4) != 0 && !visited[cordsL])
            {
                bridgeDir |= 1 << 3;
            }

            RoomEntity ent;

            switch (block.type)
            {
                case BlockType.Normal:
                    List<RoomEntity> normals = roomEntities.FindAll(a => a.type == block.type);
                    ent = normals[UnityEngine.Random.Range(0, normals.Count)];
                    break;

                default:
                    ent = roomEntities.Find(a => a.type == block.type);
                    break;
            }

            GameObject go = Instantiate(ent.prefab, parent);
            go.transform.localPosition = new Vector3(-cords.x * offset.x, 0, -cords.y * offset.y);
            go.name = Convert.ToString(bridgeDir, 2).PadLeft(4, '0') + "_" + (int)block.type + "_" + -cords.x + "_" + -cords.y;
            go.SetActive(true);
        }

    }

    public void EnemyDown()
    {
        if (targetRoom == null)
            return;

        targetRoom.RemainEnemy -= 1;

        if (targetRoom.RemainEnemy <= 0)
        {
            if (targetRoom.NextStage())
                GameResultController.Instance.FinishGame(true);
        }
    }

}

[Serializable]
struct RoomEntity
{
    public BlockType type;
    public GameObject prefab;
}