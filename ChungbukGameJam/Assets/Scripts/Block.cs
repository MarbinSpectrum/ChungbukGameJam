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
        transform.position = v + offset;
        GameManager.bv = GameManager.ConvertTileVec(transform.position);
        nowBlock = this;

        nowBlock.GetComponent<SpriteRenderer>().sortingOrder = nowBlockSortNum;

        DragDelegate.CallInvoke(false);
    }

    private void OnMouseUp()
    {
        nowBlock.GetComponent<SpriteRenderer>().sortingOrder = baseSortNum;
        DragDelegate.CallInvoke(true);

        offset = Vector2.zero;
        nowBlock = null;

        Vector2 v = transform.position;
        if (GameManager.map_size.x % 2 == 1)
            v.x += 0.5f;
        v.x = Mathf.FloorToInt(v.x);
        if (GameManager.map_size.x % 2 == 0)
            v.x += 0.5f;

        if (GameManager.map_size.y % 2 == 1)
            v.y += 0.5f;
        v.y = Mathf.FloorToInt(v.y);
        if (GameManager.map_size.y % 2 == 0)
            v.y += 0.5f;

        transform.position = v;

        
        GameManager.instance.victoryDele();
        Debug.Log("즐거운 체크 시간");
    }

    public void ReturnToBasePos()
    {
        transform.position = basePos;
    }
}
