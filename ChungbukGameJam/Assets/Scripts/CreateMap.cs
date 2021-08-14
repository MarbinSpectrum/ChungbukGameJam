using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CreateMap : SerializedMonoBehaviour
{
    private Vector2Int MAP_SIZE = new Vector2Int(1, 1);
    [Title("Å©±â")]
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
            if(MAP != null)
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

    [Title("¸Ê")]
    public bool[,] MAP;

    /////////////////////////////////////////////////////////////////

    public const float OBJ_X = 1;
    public const float OBJ_Y = 1;

    public void CreateMAP()
    {
        Tile temp = transform.GetChild(0).GetComponent<Tile>();
        float startX = -(OBJ_X * (MAP_SIZE.x - 1)) * 0.5f;
        float startY = +(OBJ_Y * (MAP_SIZE.y - 1)) * 0.5f;

        for (int r = 0; r < MAP_SIZE.y; r++)
            for (int c = 0; c < MAP_SIZE.x; c++)
            {
                Tile obj = Instantiate(temp);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(startX + c * OBJ_X, startY - r * OBJ_Y, temp.transform.position.z);
                obj.gameObject.SetActive(MAP[c, r]);
                GameManager.Tile[c, r] = obj;
            }
        temp.gameObject.SetActive(false);
    }
}
