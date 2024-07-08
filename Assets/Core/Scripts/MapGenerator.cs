using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject Container;
    public List<GameObject> Prefabs = new List<GameObject>();
    public List<GameObject> ConsumedPrefabs = new List<GameObject>();

    [SerializeField] private int m_MaxBlock = 7;

    [Header("Segment Size")]
    [SerializeField] private int m_MinSegment = 4;
    [SerializeField] private int m_MaxSegment = 5;

    [Header("Block offset")]
    [SerializeField] private Vector2 m_Offsets = new Vector2(30, 22.5f);

    private MapBlock[,] m_Blocks;
    private List<Vector2Int> m_Paths = new List<Vector2Int>();

    private Dictionary<BlockDirection, int> m_BlockDirRemap = new Dictionary<BlockDirection, int>();
    private Dictionary<BlockType, int> m_BlockTypeRemap = new Dictionary<BlockType, int>();

    private Vector2Int m_CurrentPoint;

    private void PrepareData()
    {
        if (m_Blocks != null)
        {
            for (int i = 0; i <  m_Blocks.GetLength(0); i++)
                for (int j = 0; j < m_Blocks.GetLength(1); j++)
                {
                    if (m_Blocks[i, j].go != null)
                    {
                        Destroy(m_Blocks[i, j].go);
                    }
                }
        }

        m_Blocks = new MapBlock[2 * m_MaxBlock + 4, 2 * m_MaxBlock + 4];
        for (int i = 0; i < m_Blocks.GetLength(0); i++)
            for (int j = 0; j < m_Blocks.GetLength(1); j++)
            {
                m_Blocks[i, j] = new MapBlock();
                m_Blocks[i, j].direction = BlockDirection.Empty;
            }

        m_Paths = new List<Vector2Int>();
        m_CurrentPoint = Vector2Int.zero;

        Container.transform.localPosition = Vector3.zero;
    }


    private void Awake()
    {
        m_BlockDirRemap.Add(BlockDirection.None, 0);

        m_BlockDirRemap.Add(BlockDirection.T, 1);
        m_BlockDirRemap.Add(BlockDirection.R, 2);
        m_BlockDirRemap.Add(BlockDirection.B, 3);
        m_BlockDirRemap.Add(BlockDirection.L, 4);

        m_BlockDirRemap.Add(BlockDirection.TR, 5);
        m_BlockDirRemap.Add(BlockDirection.TB, 6);
        m_BlockDirRemap.Add(BlockDirection.TL, 7);
        m_BlockDirRemap.Add(BlockDirection.RB, 8);
        m_BlockDirRemap.Add(BlockDirection.RL, 9);
        m_BlockDirRemap.Add(BlockDirection.BL, 10);

        m_BlockDirRemap.Add(BlockDirection.TRB, 11);
        m_BlockDirRemap.Add(BlockDirection.TRL, 12);
        m_BlockDirRemap.Add(BlockDirection.TBL, 13);
        m_BlockDirRemap.Add(BlockDirection.RBL, 14);

        m_BlockDirRemap.Add(BlockDirection.TRBL, 15);


        m_BlockTypeRemap.Add(BlockType.Start, 0);
        m_BlockTypeRemap.Add(BlockType.Normal, 1);
        m_BlockTypeRemap.Add(BlockType.Transition, 2);
        m_BlockTypeRemap.Add(BlockType.Boss, 3);

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        MoveLeft();
    //    else if (Input.GetKeyDown(KeyCode.RightArrow))
    //        MoveRight();
    //    else if (Input.GetKeyDown(KeyCode.UpArrow))
    //        MoveTop();
    //    else if (Input.GetKeyDown(KeyCode.DownArrow))
    //        MoveBottom();
    //}

    public void MoveLeft() => UpdateMap(MoveDirection.Left);
    public void MoveTop() => UpdateMap(MoveDirection.Top);
    public void MoveRight() => UpdateMap(MoveDirection.Right);
    public void MoveBottom() => UpdateMap(MoveDirection.Bottom);

    private void UpdateMap(MoveDirection dir)
    {
        Vector2Int targetPoint = m_CurrentPoint;
        int comparedValue = 0;

        switch (dir)
        {
            case MoveDirection.Top:
                targetPoint.y += 1;
                comparedValue = 1 << 1;
                break;
            case MoveDirection.Right:
                targetPoint.x += 1;
                comparedValue = 1 << 2;
                break;
            case MoveDirection.Bottom:
                targetPoint.y -= 1;
                comparedValue = 1 << 3;
                break;
            case MoveDirection.Left:
                targetPoint.x -= 1;
                comparedValue = 1 << 4;
                break;
        }

        if (!IsEmptyBlock(targetPoint.x, targetPoint.y) &&
            ((int)GetBlock(m_CurrentPoint.x, m_CurrentPoint.y).direction & comparedValue) != 0)
        {
            Vector3 pos = -new Vector3(targetPoint.x * m_Offsets.x, targetPoint.y * m_Offsets.y);
            Container.transform.localPosition = pos;

            if (!IsConsumedBlock(targetPoint.x, targetPoint.y))
            {
                ConsumeBlock(targetPoint.x, targetPoint.y);
            }

            m_CurrentPoint = targetPoint;
        }
        
    }

    public void Initialize(bool lastZone)
    {
        PrepareData();
                
        // Start block
        AddtoPath(0, 0, 0);

        // Base path
        int segmentSize = Random.Range(m_MinSegment, m_MaxSegment + 1);
        for (int i = 0; i < segmentSize; i++)
        {
            int x = m_Paths[i].x;
            int y = m_Paths[i].y;

            AddAdjacentBlock(x, y);
        }

        // Add branch
        int maxBranch = m_MaxBlock - segmentSize;
        for (int i = 0; i < maxBranch; ++i)
        {
            if (Random.Range(0, 1f) > 0.25f)
            {
                int segmentIndex = Random.Range(1, segmentSize + 1);

                int x = m_Paths[segmentIndex].x;
                int y = m_Paths[segmentIndex].y;

                AddAdjacentBlock(x, y);
            }
        }

        // End block
        AddAdjacentBlock(m_Paths[segmentSize].x, m_Paths[segmentSize].y);

        GetBlock(m_Paths[0].x, m_Paths[0].y).type = BlockType.Start;

        if (lastZone)
            GetBlock(m_Paths[m_Paths.Count - 1].x, m_Paths[m_Paths.Count - 1].y).type = BlockType.Boss;
        else
            GetBlock(m_Paths[m_Paths.Count - 1].x, m_Paths[m_Paths.Count - 1].y).type = BlockType.Transition;

        ConsumeBlock(0, 0);
        UpdateMap(MoveDirection.None);
    }

    private void AddAdjacentBlock(int originalX, int originalY)
    {
        int i = 0;
        do
        {
            int x = originalX;
            int y = originalY;
            int originalDir = (int)GetBlock(x, y).direction;

            int adjacentDir = 0;

            MoveDirection dir = (MoveDirection)Random.Range(0, 4);
            switch (dir)
            {
                case MoveDirection.Top:
                    originalDir |= 1 << 1;
                    adjacentDir |= 1 << 3;
                    y += 1;
                    break;
                case MoveDirection.Right:
                    originalDir |= 1 << 2;
                    adjacentDir |= 1 << 4;
                    x += 1;
                    break;
                case MoveDirection.Bottom:
                    originalDir |= 1 << 3;
                    adjacentDir |= 1 << 1;
                    y -= 1;
                    break;
                case MoveDirection.Left:
                    originalDir |= 1 << 4;
                    adjacentDir |= 1 << 2;
                    x -= 1;
                    break;
            }

            if (i == 100)
                Debug.Log("????");

            if (IsEmptyBlock(x, y))
            {
                AddtoPath(x, y, adjacentDir);
                GetBlock(originalX, originalY).direction = (BlockDirection)originalDir;
                break;
            }
        }
        while (i++ < 100);
    }

    private void AddtoPath(int x, int y, int blockDir)
    {
        m_Paths.Add(new Vector2Int(x, y));
        GetBlock(x, y).direction = (BlockDirection)blockDir;
    }

    private void DrawAdjacentBlocks(int originalX, int originalY, bool isHidden = true)
    {
        BlockDirection blockDirection = GetBlock(originalX, originalY).direction;
        int blockDir = (int)GetBlock(originalX, originalY).direction;

        DrawBlock(originalX, originalY, blockDirection, true);

        int xT = originalX;
        int yT = originalY + 1;
        if (!IsEmptyBlock(xT, yT) && (blockDir & 1 << 1) != 0 && !IsConsumedBlock(xT, yT))
        {
            DrawBlock(xT, yT, BlockDirection.B);
        }

        int xR = originalX + 1;
        int yR = originalY;
        if (!IsEmptyBlock(xR, yR) && (blockDir & 1 << 2) != 0 && !IsConsumedBlock(xR, yR))
        {
            DrawBlock(xR, yR, BlockDirection.L);
        }

        int xB = originalX;
        int yB = originalY - 1;
        if (!IsEmptyBlock(xB, yB) && (blockDir & 1 << 3) != 0 && !IsConsumedBlock(xB, yB))
        {
            DrawBlock(xB, yB, BlockDirection.T);
        }

        int xL = originalX - 1;
        int yL = originalY;
        if (!IsEmptyBlock(xL, yL) && (blockDir & 1 << 4) != 0 && !IsConsumedBlock(xL, yL) )
        {
            DrawBlock(xL, yL, BlockDirection.R);
        }
    }

    private void DrawBlock(int x, int y, BlockDirection blockDir, bool consumed = false)
    {
        MapBlock block = GetBlock(x, y);
        GameObject oldObj = block.go;
        GameObject obj = Instantiate(Prefabs[m_BlockDirRemap[blockDir] - 1], Container.transform);

        Vector3 pos = new Vector3(x * m_Offsets.x, y * m_Offsets.y, 0);
        Vector3 scale = Vector3.one;

        obj.transform.localPosition = pos;
        obj.transform.localScale = scale;

        GetBlock(x, y).go = obj;

        if (consumed)
        {
            Destroy(oldObj);
            BlockType type = block.type;
            Instantiate(ConsumedPrefabs[m_BlockTypeRemap[type]], obj.transform);
        }
    }

    private void ConsumeBlock(int originalX, int originalY, bool isHidden = true)
    {
        DrawAdjacentBlocks(originalX, originalY, isHidden);

        GetBlock(originalX, originalY).consumed = true;
    }

    public MapBlock GetBlock(int x, int y) => m_Blocks[x + m_MaxBlock + 1, y + m_MaxBlock + 1];
    public List<Vector2Int> GetPath() => m_Paths;

    public bool IsConsumedBlock(int x, int y) => m_Blocks[x + m_MaxBlock + 1, y + m_MaxBlock + 1].consumed;

    public bool IsEmptyBlock(int x, int y) => m_Blocks[x + m_MaxBlock + 1, y + m_MaxBlock + 1].direction == BlockDirection.Empty;

    private IEnumerator DrawDebug(int segmentSize)
    {
        for (int i = 0; i < m_Paths.Count; ++i)
        {
            BlockDirection blockDir = GetBlock(m_Paths[i].x, m_Paths[i].y).direction;
            GameObject obj = Instantiate(Prefabs[m_BlockDirRemap[blockDir] - 1], Container.transform);
            Vector3 pos = new Vector3(m_Paths[i].x * m_Offsets.x, m_Paths[i].y * m_Offsets.y, 0);
            Vector3 scale = Vector3.one;

            obj.transform.localPosition = pos;
            obj.transform.localScale = scale;

            //MeshRenderer mr = obj.GetComponent<MeshRenderer>();

            //if (i == 0)
            //    mr.material.color = Color.cyan;
            //else if (i == m_Paths.Count - 1)
            //    mr.material.color = Color.red;
            //else if (i > segmentSize)
            //    mr.material.color = Color.green;
            //else
            //    mr.material.color = Color.white;

            yield return new WaitForSeconds(0.1f);
        }
    }

}

public class MapBlock
{
    public BlockDirection direction;
    public BlockType type = BlockType.Normal;
    public bool consumed;
    public GameObject go;

    public MapBlock() { }

    public MapBlock(BlockDirection direction, BlockType type, bool consumed, GameObject go)
    {
        this.direction = direction;
        this.type = type;
        this.consumed = consumed;
        this.go = go;
    }
}

public enum MoveDirection
{
    None = -1, Top, Right, Bottom, Left
}

public enum BlockDirection
{
    T = 1 << 1,
    R = 1 << 2,
    B = 1 << 3,
    L = 1 << 4,

    TR = 1 << 1 | 1 << 2,
    TB = 1 << 1 | 1 << 3,
    TL = 1 << 1 | 1 << 4,
    RB = 1 << 2 | 1 << 3,
    RL = 1 << 2 | 1 << 4,
    BL = 1 << 3 | 1 << 4,

    TRB = 1 << 1 | 1 << 2 | 1 << 3,
    TRL = 1 << 1 | 1 << 2 | 1 << 4,
    TBL = 1 << 1 | 1 << 3 | 1 << 4,
    RBL = 1 << 2 | 1 << 3 | 1 << 4,

    TRBL = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4,

    Empty = -1,
    None = 0
}

public enum BlockType
{
    Start, Normal, Transition, Boss
}