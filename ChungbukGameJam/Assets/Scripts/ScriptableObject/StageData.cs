using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "ScriptableObjects/New Stage")]
public class StageData : SerializedScriptableObject
{
    [BoxGroup("Basic Stage Infomation")]
    public int stageID = 0;
    public string stageName;

    public List<BlockCollection> blocks = new List<BlockCollection>();

    private Vector2Int MAP_SIZE = new Vector2Int(1, 1);
    [Title("TileMapSize")]
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
                MAP = new bool[MAP_SIZE.x, MAP_SIZE.y];
            
        }
    }

    [Title("TileMap")]
    public bool[,] MAP;
}

[System.Serializable]
public class BlockCollection
{
    public Block block;
    public int num;
}
