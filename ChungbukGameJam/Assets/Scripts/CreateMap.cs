using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CreateMap : SerializedMonoBehaviour
{
    public static CreateMap instance;

    private Vector2Int MAP_SIZE = new Vector2Int(1, 1);
    [Title("ũ��")]
    [GUIColor(0, 1, 0)]
    [HideLabel]
    [SerializeField]
    public Vector2Int map_size
    {
        get { return MAP_SIZE; }
        set
        {
            value.x = Mathf.Max(1, value.x);
            value.y = Mathf.Max(1, value.y);
            MAP_SIZE = value;
            if (MAP != null)
            {
                bool[,] tempMap = new bool[MAP_SIZE.x, MAP_SIZE.y];
                for (int r = 0; r < Mathf.Min(MAP_SIZE.y, MAP.GetLength(1)); r++)
                    for (int c = 0; c < Mathf.Min(MAP_SIZE.x, MAP.GetLength(0)); c++)
                        tempMap[c, r] = MAP[c, r];
                MAP = tempMap;
            }
            else
            {
                MAP = new bool[MAP_SIZE.x, MAP_SIZE.y];
            }
        }
    }

    [Title("��")]
    public bool[,] MAP;

    /////////////////////////////////////////////////////////////////

    public const float OBJ_X = 0.5f;
    public const float OBJ_Y = 0.5f;

    private void Awake()
    {
        instance = this;

        // Vector2 basePos = Vector2.zero;
        // basePos.x = -Block.enlargeRate * (MAP_SIZE.x) * 0.5f;
        // basePos.y = Block.enlargeRate * (MAP_SIZE.y);
        // transform.position = basePos + Vector2.one * Block.enlargeRate * 0.5f; 
        //new Vector2(-Block.enlargeRate * 0.5f, -Block.enlargeRate * 0.5f);
    }

    public void CreateMAP()
    {
        Tile temp = transform.GetChild(0).GetComponent<Tile>();
        float startX = -(Block.enlargeRate * (MAP_SIZE.x - 1)) * 0.5f;
        float startY = +(Block.enlargeRate * (MAP_SIZE.y - 1)) * 0.5f;

        for (int r = 0; r < MAP_SIZE.y; r++)
            for (int c = 0; c < MAP_SIZE.x; c++)
            {
                Tile obj = Instantiate(temp);
                obj.transform.SetParent(transform);
                obj.transform.localScale *= Block.enlargeRate;
                obj.transform.localPosition = new Vector3(c * Block.enlargeRate + startX, - r * Block.enlargeRate + startY, temp.transform.position.z);
                obj.gameObject.SetActive(MAP[c, r]);
                GameManager.Tile[c, r] = obj;
            }
        temp.gameObject.SetActive(false);
    }
}