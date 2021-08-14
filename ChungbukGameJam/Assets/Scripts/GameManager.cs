using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Tile[,] Tile;
    public static Vector2 bv = new Vector2(-10, -10);
    public static Vector2Int map_size;
    private CreateMap createMap;

    private void Awake()
    {
        instance = this;
        createMap = FindObjectOfType<CreateMap>();
        Tile = new Tile[createMap.map_size.x, createMap.map_size.y];
        createMap.CreateMAP();
        map_size = createMap.map_size;
    }

    public static Vector2 ConvertTileVec(Vector2 v)
    {
        v.x += (CreateMap.OBJ_X * (map_size.x - 1)) * 0.5f;
        v.x += 0.5f;
        v.x = Mathf.FloorToInt(v.x);

        v.y = -v.y;
        v.y += (CreateMap.OBJ_Y * (map_size.y - 1)) * 0.5f;
        v.y += 0.5f;
        v.y = Mathf.FloorToInt(v.y);
        return v;
    }



    public void Update()
    {
        bool[,] tileState = new bool[map_size.x, map_size.y];
        if (Block.nowBlock)
        {
            for (int r = Mathf.Max(0, (int)bv.y); r < Mathf.Min(createMap.map_size.y, bv.y + Block.nowBlock.block_size); r++)
                for (int c = Mathf.Max(0,(int)bv.x); c < Mathf.Min(createMap.map_size.x, bv.x + Block.nowBlock.block_size); c++)
                {
                    int ac = c - (int)bv.x;
                    int ar = r - (int)bv.y;
                    if (ac < 0 || ar < 0 || ac >= Block.nowBlock.block_size || ar >= Block.nowBlock.block_size)
                        continue;
                    if (Block.nowBlock.MAP[ac, ar])
                        tileState[c, r] = true;

                }
        }


        for (int r = 0; r < map_size.y; r++)
            for (int c = 0; c < map_size.x; c++)
            {
                if (tileState[c, r])
                    Tile[c, r].Highight();
                else
                    Tile[c, r].Normal();
            }
    }
}
