using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal static class DragDelegate
{
    public delegate void Dele(bool b);
    static event Dele BlockCheck;

    public static void CallInvoke(bool isRelease)
    {
        BlockCheck?.Invoke(isRelease);
    }

    public static void SubscribeBlockCheck(Dele dele)
    {
        BlockCheck += dele;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Tile[,] Tile;
    public static Vector2 bv = new Vector2(-10, -10);
    public static Vector2Int map_size;
    private CreateMap createMap;

    public delegate bool tileDelegate();
    internal tileDelegate victoryDele;

    [SerializeField]
    List<Tile> landingCheckTiles = new List<Tile>();

    private void Awake()
    {
        instance = this;
        createMap = FindObjectOfType<CreateMap>();

        Tile = new Tile[createMap.map_size.x, createMap.map_size.y];

        createMap.CreateMAP();
        map_size = createMap.map_size;

        DragDelegate.SubscribeBlockCheck(SettingCheck);
        victoryDele += CheckVictory;
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

    // 타일 내 색깔 변경은 optional featrue로 판정.
    // 기획팀과 상의한 뒤, 이 기능이 필요한 지 물어보도록 하자.
    public void SettingCheck(bool isRelease)
    {
        bool[,] tileState = new bool[map_size.x, map_size.y];

        if (Block.nowBlock)
        {
            for (int r = Mathf.Max(0, (int)bv.y); r < Mathf.Min(createMap.map_size.y, bv.y + Block.nowBlock.block_size); r++)
                for (int c = Mathf.Max(0, (int)bv.x); c < Mathf.Min(createMap.map_size.x, bv.x + Block.nowBlock.block_size); c++)
                {
                    // c, r: 월드 좌표에 해당되는 타일의 위치
                    
                    if(!Block.nowBlock.MAP[c,r]) continue;

                    // 타일맵 좌표
                    int ac = c - (int)bv.x;
                    int ar = r - (int)bv.y;
                    if (ac < 0 || ar < 0 || ac >= Block.nowBlock.block_size || ar >= Block.nowBlock.block_size)
                    {
                        Block.nowBlock.ReturnToBasePos();
                        continue;
                    }

                    if (Block.nowBlock.MAP[ac, ar])
                    {
                        tileState[c, r] = true;
                    }

                    if (isRelease)
                    {
                        if (tileState[c, r])
                            if (Tile[c, r].GetIsFill()) // if(Tile[c, r].GetIsFill())
                                Block.nowBlock.ReturnToBasePos();
                            else if(CheckCanTileLand(Tile[c, r]))
                                Tile[c, r].SetIsFill(isRelease);

                        if (!landingCheckTiles.Contains(Tile[c, r]))
                            landingCheckTiles.Add(Tile[c, r]);
                    }
                }

            for (int r = 0; r < map_size.y; r++)
                for (int c = 0; c < map_size.x; c++)
                    ChangeTileColorInMap(tileState, c, r);

        }
    }

    void ChangeTileColorInMap(bool[,] tileConditions, int x, int y)
    {
        if (tileConditions[x, y] || Tile[x, y].GetIsFill())
            Tile[x, y].Highight();
        else
            Tile[x, y].Normal();
    }

    bool CheckCanTileLand(Tile t)
    {
        for (int i = 0; i < landingCheckTiles.Count; i++)
        {
            if (landingCheckTiles.Contains(t))
                return false;
        }
        return true;
    }

    internal bool CheckVictory()
    {
        for (int w = 0; w < Tile.GetLength(0); w++)
        {
            for (int h = 0; h < Tile.GetLength(1); h++)
            {
                if (Tile[w, h].GetIsFill() == false)
                    return false;
            }
        }

        VictoryComment();
        return true;
    }

    private void VictoryComment()
    {
        Debug.Log("승리!");
    }


    // public void Update()
    // {
    //     bool[,] tileState = new bool[map_size.x, map_size.y];
    //     if (Block.nowBlock)
    //     {
    //         for (int r = Mathf.Max(0, (int)bv.y); r < Mathf.Min(createMap.map_size.y, bv.y + Block.nowBlock.block_size); r++)
    //             for (int c = Mathf.Max(0, (int)bv.x); c < Mathf.Min(createMap.map_size.x, bv.x + Block.nowBlock.block_size); c++)
    //             {
    //                 int ac = c - (int)bv.x;
    //                 int ar = r - (int)bv.y;
    //                 if (ac < 0 || ar < 0 || ac >= Block.nowBlock.block_size || ar >= Block.nowBlock.block_size)
    //                     continue;
    //                 if (Block.nowBlock.MAP[ac, ar])
    //                     tileState[c, r] = true;

    //             }
    //     }


    //     for (int r = 0; r < map_size.y; r++)
    //         for (int c = 0; c < map_size.x; c++)
    //         {
    //             if (tileState[c, r])
    //                 Tile[c, r].Highight();
    //             else
    //                 Tile[c, r].Normal();
    //         }
    // }
}
