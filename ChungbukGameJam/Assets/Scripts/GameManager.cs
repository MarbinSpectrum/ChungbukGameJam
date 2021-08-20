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
    public List<Block> blockData = new List<Block>();

    private void Awake()
    {
        if (instance != null)
            return;

        instance = this;
        createMap = FindObjectOfType<CreateMap>();

        Tile = new Tile[createMap.map_size.x, createMap.map_size.y];

        createMap.CreateMAP();
        map_size = createMap.map_size;

        DragDelegate.SubscribeBlockCheck(SettingCheck);
        victoryDele += CheckVictory;
    }

    public static Vector2 ConvertCeilVec(Vector2 v)
    {
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
        return v;
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

    //선택된 블록이 내부에 몇개나 있는지 검사
    public static int InTheBlockCount(Block block,List<Vector2> list = null)
    {
        int res = 0;

        HashSet<Vector2> Set = new HashSet<Vector2>();

        for (int c = 0; c < instance.createMap.map_size.x; c++)
            for (int r = 0; r < instance.createMap.map_size.y; r++)
                if (instance.createMap.MAP[c, r])
                    Set.Add(instance.createMap.transform.position + Tile[c, r].transform.localPosition);

        if(list == null)
            list = block.GetBlocksArray();

        foreach (Vector2 vec in list)
            if (Set.Contains(vec))
                res++;

        return res;
    }

    private void CheckTile()
    {     
        HashSet<Vector2> Set = new HashSet<Vector2>();
        foreach (Block b in blockData)
            for (int c = 0; c < b.block_size; c++)
                for (int r = 0; r < b.block_size; r++)
                    if (b.MAP[c, r])
                    {
                        Vector2 mainPos = b.transform.position;
                        Vector2 temp = new Vector2(c, -r);
                        Set.Add(new Vector2(mainPos.x + temp.x, mainPos.y + temp.y));
                    }

        for (int c = 0; c < createMap.map_size.x; c++)
            for (int r = 0; r < createMap.map_size.y; r++)
                if (createMap.MAP[c, r])
                    Tile[c, r].SetIsFill(Set.Contains(createMap.transform.position + Tile[c, r].transform.localPosition));
    }


    // 타일 내 색깔 변경은 optional featrue로 판정.
    // 기획팀과 상의한 뒤, 이 기능이 필요한 지 물어보도록 하자.
    public void SettingCheck(bool isRelease)
    {
        bool[,] tileState = new bool[map_size.x, map_size.y];
        if (Block.nowBlock)
        {
            if(!isRelease)
            {
                for (int r = 0; r < map_size.y; r++)
                    for (int c = 0; c < map_size.x; c++)
                        Tile[c, r].SetIsFill(false);

                for (int r = Mathf.Max(0, (int)bv.y); r < Mathf.Min(createMap.map_size.y, bv.y + Block.nowBlock.block_size); r++)
                    for (int c = Mathf.Max(0, (int)bv.x); c < Mathf.Min(createMap.map_size.x, bv.x + Block.nowBlock.block_size); c++)
                    {
                        // 타일맵 좌표
                        int ac = c - (int)bv.x;
                        int ar = r - (int)bv.y;
                        if (Block.nowBlock.MAP[ac, ar])
                            tileState[c, r] = true;
                        else
                            tileState[c, r] = false;
                    }

                for (int r = 0; r < map_size.y; r++)
                    for (int c = 0; c < map_size.x; c++)
                        ChangeTileColorInMap(tileState, c, r);

            }
            else 
            {
                int inTheBlockCount = InTheBlockCount(Block.nowBlock);
                if ((0 < inTheBlockCount && inTheBlockCount < Block.nowBlock.GetBlockCount()) || Block.nowBlock.OverLapBlock())
                {
                    Block.nowBlock.ReturnToBasePos();
                    Array.Clear(tileState, 0, tileState.Length);
                    ChangeTileColorInMaps(tileState);

                }
                else
                    CheckTile();
            }
        }
    }


    void ChangeTileColorInMaps(bool[,] tileConditions)
    {
        for (int w = 0; w < Tile.GetLength(0); w++)
            for (int h = 0; h < Tile.GetLength(1); h++)
                if (tileConditions[w, h])
                    Tile[w, h].Highight();
                else
                    Tile[w, h].Normal();
    }
    void ChangeTileColorInMap(bool[,] tileConditions, int x, int y)
    {
        if (tileConditions[x, y])
            Tile[x, y].Highight();
        else
            Tile[x, y].Normal();
    }

    internal bool CheckVictory()
    {
        for (int w = 0; w < Tile.GetLength(0); w++)
            for (int h = 0; h < Tile.GetLength(1); h++)
                if (createMap.MAP[w, h])
                    if (Tile[w, h].GetIsFill() == false)
                        return false;

        VictoryComment();
        return true;
    }

    private void VictoryComment()
    {
        Debug.Log("승리!");
    }
}
