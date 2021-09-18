using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class Block : SerializedMonoBehaviour
{
    public const float shrinkRate = .35f;
    public const float enlargeRate = 0.75f;
    public const int baseSortNum = 0;
    public const int nowBlockSortNum = 99;

    // 다른 블록, 설치 불가능한 지역에 놓여질 때 되돌아갈 위치.
    private Vector2 basePos;

    public static Block nowBlock;
    public float curSize = 0;
    public float sizeControllY;
    private int Block_size = 1;
    static RaycastHit2D hit2D;
    CompositeCollider2D compositeCollider2D;

    bool isSizeModified = false;

    [Title("크기")]
    [GUIColor(0, 1, 0)]
    [HideLabel]
    [SerializeField]
    public int block_size
    {
        get { return Block_size; }
        set
        {
            value = Mathf.Max(1, value);
            Block_size = value;
            MAP = new bool[Block_size, Block_size];
        }
    }

    [Title("모양")]
    public bool[,] MAP;
    

    /////////////////////////////////////////////////////////////////

    // private const float OBJ_X = 1f;
    // private const float OBJ_Y = 1f;

    private Vector2 offset;
    GameObject[,] blocks;

    List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private void Start()
    {
        curSize = shrinkRate;
        compositeCollider2D = GetComponent<CompositeCollider2D>();

        GameManager.instance.blockData.Add(this);
        SortBlock.instance.sortRankBlock.Add(this);
        SortBlock.instance.SortBlocks();

        GameObject temp = transform.GetChild(0).gameObject;
        blocks = new GameObject[block_size, block_size];

        for (int r = 0; r < Block_size; r++)
            for (int c = 0; c < Block_size; c++)
            {
                GameObject obj = Instantiate(temp);
                obj.transform.SetParent(transform);
                obj.transform.localScale *= curSize;
                obj.transform.localPosition = new Vector3(c * curSize, -r * curSize, temp.transform.position.z);
                blocks[c, r] = obj;
                spriteRenderers.Add(obj.GetComponent<SpriteRenderer>());
            }

        UpdateBlockState();
        temp.SetActive(false);

        transform.position = GameManager.ConvertCeilVec(transform.position);
    }

    public void UpdateBlockState()
    {
        for (int r = 0; r < Block_size; r++)
            for (int c = 0; c < Block_size; c++)
                blocks[c, r].SetActive(MAP[c, r]);
    }

    public void TouchBlock()
    {
        ControllSize(enlargeRate);
    }

    public void DragBlock(Vector2 v)
    {
        // nowBlock = this;

        if (offset == Vector2.zero)
            offset = (Vector2)transform.position - v;

        transform.position = v + offset;

        GameManager.bv = GameManager.ConvertTileVec(transform.position);

        foreach (SpriteRenderer sprite in spriteRenderers)
            sprite.sortingOrder = nowBlockSortNum;

        ShadowDelegate.CallInvoke(false);

        if (SortBlock.instance.sortRankBlock.Contains(this))
            SortBlock.instance.sortRankBlock.Remove(this);

        nowBlock.GetComponent<SpriteRenderer>().sortingOrder = nowBlockSortNum;
        SortBlock.instance.SortOrderDuringDrag(this);
    }

    public void SetBlockToPos()
    {
        foreach (SpriteRenderer sprite in spriteRenderers)
            sprite.sortingOrder = baseSortNum;

        offset = Vector2.zero;
        // clickPos = Vector2.zero;
        transform.position = GameManager.ConvertCeilVec(transform.position);

        if (sizeControllY < transform.position.y * enlargeRate)
            ControllSize(enlargeRate);
        else
            ControllSize(shrinkRate);

        ShadowDelegate.CallInvoke(true);
        nowBlock = null;

        GameManager.InvokeCheckVictoryDele();

        if (!SortBlock.instance.sortRankBlock.Contains(this))
            SortBlock.instance.sortRankBlock.Add(this);
        SortBlock.instance.SortBlocks();
    }

    // private Vector2 clickPos;
    private Vector2 Rotate(Vector2 p, Vector2 pivot, float angle)
    {
        Vector2 ret;
        angle = angle * Mathf.Deg2Rad;
        ret.x = (p.x - pivot.x)  * Mathf.Cos(angle) - (p.y - pivot.y) * Mathf.Sin(angle) + pivot.x;
        ret.y = (p.x - pivot.x)  * Mathf.Sin(angle) + (p.y - pivot.y) * Mathf.Cos(angle) + pivot.y;

        return ret;
    }

    public int GetBlockCount()
    {
        int res = 0;
        for (int c = 0; c < block_size; c++)
            for (int r = 0; r < block_size; r++)
                if (MAP[c, r])
                    res++;
        return res;
    }

    public List<Vector2> GetBlocksArray()
    {
        List<Vector2> list = new List<Vector2>();
        Vector2 mainPos = GameManager.ConvertCeilVec(transform.position);
        for (int c = 0; c < block_size; c++)
            for (int r = 0; r < block_size; r++)
                if (MAP[c, r])
                {
                    Vector2 temp = blocks[c, r].transform.localPosition;
                    Vector2 newVec = new Vector2(mainPos.x + temp.x, mainPos.y + temp.y);
                    list.Add(newVec);
                }

        return list;
    }
    public bool OverLapBlock()
    {
        return OverLapBlock(GetBlocksArray());
    }

    public bool OverLapBlock(List<Vector2> list)
    {
        HashSet<Vector2> Set = new HashSet<Vector2>();
        var blockList = GameManager.instance.blockData;
        foreach (Block b in blockList)
        {
            if (b == this)
                continue;
            for (int c = 0; c < b.block_size; c++)
                for (int r = 0; r < b.block_size; r++)
                    if (b.MAP[c, r])
                    {
                        Vector2 mainPos = b.transform.position;
                        Vector2 temp = new Vector2(c, -r) * curSize;
                        Set.Add(new Vector2(mainPos.x + temp.x, mainPos.y + temp.y));
                    }
        }

        foreach (Vector2 vec in list)
            if (Set.Contains(vec))
                return true;
    
        return false;
    }
    private bool CanRotate(Vector2 clickPos)
    {
        List<Vector2> list = new List<Vector2>();

        clickPos = GameManager.ConvertCeilVec(clickPos);
        Vector2 pivot = (Vector2)transform.position + new Vector2((block_size - 1), -(block_size - 1)) * curSize * 0.5f;
        Vector2 newCenterPos = Rotate(clickPos, pivot, -90);
        Vector2 offsetTemp = newCenterPos - clickPos;
        Vector2 mainPos = (Vector2)transform.position - offsetTemp;
        mainPos = new Vector2(mainPos.x, mainPos.y);

        for (int c = 0; c < block_size; c++)
            for (int r = 0; r < block_size; r++)
                if (MAP[c, r])
                {
                    Vector2 temp = blocks[block_size - 1 - r, c].transform.localPosition;
                    Vector2 newVec = new Vector2(mainPos.x + temp.x, mainPos.y + temp.y);
                    list.Add(newVec);
                }

        int inTheBlockCount = GameManager.InTheBlockCount(this, list);
        return !((0 < inTheBlockCount && inTheBlockCount < GetBlockCount()) || OverLapBlock(list));
    }

    public void RotateBlock(Vector2 clickPos)
    {
        if (Block.nowBlock != this) return;

        if (CanRotate(clickPos))
        {
            Vector2 centerPos = new Vector2(GetBlockRealSize().Item1 - 1, GetBlockRealSize().Item2 - 1) * 0.5f;
            // Vector2 centerPos = Vector2.one * (block_size - 1) / 2;

            bool[,] temp = new bool[Block_size, Block_size];
            for (int c = 0; c < block_size; c++)
                for (int r = 0; r < block_size; r++)
                {
                    temp[block_size - 1 - r, c] = MAP[c, r];
                }

            for (int c = 0; c < block_size; c++)
                for (int r = 0; r < block_size; r++)
                    MAP[c, r] = temp[c, r];

            UpdateBlockState();

            clickPos = GameManager.ConvertCeilVec(clickPos);
            Vector2 pivot = (Vector2)transform.position + new Vector2((block_size - 1), -(block_size - 1)) * curSize * 0.5f;
            Vector2 newCenterPos = Rotate(clickPos, pivot, -90);
            Vector3 offsetTemp = newCenterPos - clickPos;

            transform.position -= offsetTemp;

            GameManager.bv = GameManager.ConvertTileVec(transform.position);

            ShadowDelegate.CallInvoke(false);
        }
    }

    public (int, int) GetBlockRealSize()
    {
        int realX = 0, realY = 0;

        for (int x = 0; x < block_size; x++)
            for (int y = 0; y < block_size; y++)
                if (MAP[x, y])
                {
                    if (x >= realX)
                        realX = x + 1;

                    if (y >= realY)
                        realY = y + 1;
                }
        return (realX, realY);
    }

    public void ControllSize(float rate)
    {
        for (int r = 0; r < blocks.GetLength(1); r++)
            for (int c = 0; c < blocks.GetLength(0); c++)
            {
                blocks[c, r].gameObject.transform.localScale = Vector3.one;
                blocks[c, r].gameObject.transform.localScale *= rate;

                blocks[c, r].gameObject.transform.localPosition = new Vector3(c, -r, 0);
                blocks[c, r].gameObject.transform.localPosition *= rate;
            }

        curSize = rate;

        if (rate > enlargeRate - shrinkRate)
            isSizeModified = true;
        else
            isSizeModified = false;
    }


    public void ReturnToBasePos()
    {
        ControllSize(shrinkRate);
        transform.position = basePos;
    } 

    public void SetBasePos(Vector2 vec) => basePos = vec; 
}