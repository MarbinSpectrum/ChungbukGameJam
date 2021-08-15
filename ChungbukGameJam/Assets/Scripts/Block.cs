using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Block : SerializedMonoBehaviour
{
    public const int baseSortNum = 0;
    public const int nowBlockSortNum = -5;

    // 다른 블록, 설치 불가능한 지역에 놓여질 때 되돌아갈 위치.
    public Vector2 basePos;

    public static Block nowBlock;
    private int Block_size = 1;
    [Title("ũ��")]
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

    [Title("���")]
    public bool[,] MAP;

    /////////////////////////////////////////////////////////////////

    private const float OBJ_X = 1;
    private const float OBJ_Y = 1;

    GameObject[,] blocks;
    public void Start()
    {
        GameObject temp = transform.GetChild(0).gameObject;
        blocks = new GameObject[block_size, block_size];
        for (int r = 0; r < Block_size; r++)
            for (int c = 0; c < Block_size; c++)
            {
                GameObject obj = Instantiate(temp);
                obj.transform.SetParent(transform);
                basePos = new Vector3(c * OBJ_X, -r * OBJ_Y, temp.transform.position.z);
                obj.transform.localPosition = basePos;
                blocks[c, r] = obj;
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

    private Vector2 offset;

    private void OnMouseDrag()
    {
        Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (offset == Vector2.zero)
            offset = (Vector2)transform.position - v;
        if (clickPos == Vector2.zero)
            clickPos = v;
        transform.position = v + offset;
        GameManager.bv = GameManager.ConvertTileVec(transform.position);
        nowBlock = this;

        nowBlock.GetComponent<SpriteRenderer>().sortingOrder = nowBlockSortNum;

        DragDelegate.CallInvoke(false);
    }

    private void OnMouseUp()
    {
        if(nowBlock)
        nowBlock.GetComponent<SpriteRenderer>().sortingOrder = baseSortNum;
        DragDelegate.CallInvoke(true);

        offset = Vector2.zero;
        clickPos = Vector2.zero;

        nowBlock = null;

        transform.position = GameManager.ConvertCeilVec(transform.position);

        
        GameManager.instance.victoryDele();
        Debug.Log("즐거운 체크 시간");
    }

    private Vector2 clickPos;
    private Vector2 Rotate(Vector2 p, Vector2 pivot, float angle)
    {
        Vector2 ret;
        angle = angle * Mathf.Deg2Rad;
        ret.x = (p.x - pivot.x) * Mathf.Cos(angle) - (p.y - pivot.y) * Mathf.Sin(angle) + pivot.x;
        ret.y = (p.x - pivot.x) * Mathf.Sin(angle) + (p.y - pivot.y) * Mathf.Cos(angle) + pivot.y;

        return ret;
    }

    private void OnMouseUpAsButton()
    {
        if (Vector2.Distance(clickPos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) <= 0.01f)
        {
            Vector2 centerPos = Vector2.one * (block_size - 1) / 2;
            bool[,] temp = new bool[Block_size, Block_size];
            for (int c = 0; c < block_size; c++)
                for (int r = 0; r < block_size; r++)
                    temp[block_size - 1 - r, c] = MAP[c, r];
            for (int c = 0; c < block_size; c++)
                for (int r = 0; r < block_size; r++)
                    MAP[c, r] = temp[c, r];
            UpdateBlockState();

            clickPos = GameManager.ConvertCeilVec(clickPos);
            Vector2 pivot = (Vector2)transform.position + new Vector2((block_size - 1) * 0.5f, -(block_size - 1) * 0.5f);
            Vector2 newCenterPos = Rotate(clickPos, pivot, -90);
            Vector3 offsetTemp = newCenterPos - clickPos;
            transform.position -= offsetTemp;

        }
        clickPos = Vector2.zero;

        GameManager.bv = GameManager.ConvertTileVec(transform.position);
        DragDelegate.CallInvoke(false);
    }

    public void ReturnToBasePos()
    {
        transform.position = basePos;
    }
}
