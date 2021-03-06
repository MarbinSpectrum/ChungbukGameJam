using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal static class SettingCheckDelegate
{
    public delegate void Dele(bool b);
    public static event Dele BlockCheck;

    public static void CallInvoke(bool isRelease) => BlockCheck?.Invoke(isRelease);

    public static void SubscribeBlockCheck(Dele dele) => BlockCheck += dele;

}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CreateMap createMap;
    private StageManager stageManager;
    private TouchChecker touchChecker;

    public static Tile[,] Tile;
    public static Vector2 bv = new Vector2(-10, -10);
    public static Vector2Int map_size;
    public List<Block> blockData = new List<Block>();

    public delegate void tileDelegate();
    internal static event tileDelegate CheckVictoryDele;

    private void Awake()
    {
        if (instance != null)
            return;

        instance = this;
        createMap = FindObjectOfType<CreateMap>();
        stageManager = FindObjectOfType<StageManager>();
        touchChecker = FindObjectOfType<TouchChecker>();

        // storeTileMap.CreateOutline();
        // storeTileMap.AllocatePosToBlock();

        createMap.map_size = stageManager.stageData.map_size;
        map_size = createMap.map_size;

        Tile = new Tile[createMap.map_size.x, createMap.map_size.y];
        createMap.CreateMAP();

        SettingCheckDelegate.SubscribeBlockCheck(SettingCheck);
        CheckVictoryDele += CheckVictory;
    }

    public static Vector2 ConvertCeilVec(Vector2 v)
    {
        if (!Block.nowBlock) return v;

        float tempX = (v.x / Block.nowBlock.curSize);
        float tempY = (v.y / Block.nowBlock.curSize);

        float compareX = (int)tempX * Block.nowBlock.curSize;
        float compareY = (int)tempY * Block.nowBlock.curSize;



        if (CreateMap.instance.map_size.x % 2 == 0)
        {
            if (v.x > 0)
                v.x = compareX + 0.5f * Block.nowBlock.curSize;
            else
                v.x = compareX - 0.5f * Block.nowBlock.curSize;
        }
        else
        {
            if (v.x > 0)
            {
                // print("v.x > 0 & " + (v.x - compareX).ToString("F4"));
                if (v.x - compareX > Block.nowBlock.curSize * 0.5f) // ?????? ?????? ?????????
                    v.x = compareX + Block.nowBlock.curSize;// Block.nowBlock.curSize;
                else
                    v.x = compareX;
            }
            else
            {
                // print("v.x <= 0 & " + (v.x - compareX));
                if (v.x - compareX < -Block.nowBlock.curSize * 0.5f) // ?????? ????????? ????????? ??????.
                    v.x = compareX - Block.nowBlock.curSize;
                else
                    v.x = compareX;
            }
        }


        if (CreateMap.instance.map_size.y % 2 == 0)
        {
            if (v.y > 0)
                v.y = compareY + 0.5f * Block.nowBlock.curSize;
            else
                v.y = compareY - 0.5f * Block.nowBlock.curSize;
        }
        else
        {
            if (v.y > 0)
            {
                // print("v.y > 0 & " + +(v.y - compareY));
                if (v.y - compareY > Block.nowBlock.curSize * 0.5f) // ?????? ?????? ?????????
                    v.y = compareY + Block.nowBlock.curSize; // + Block.nowBlock.curSize;
                else
                    v.y = compareY;
            }
            else
            {
                // print("v.y <= 0 & " + (v.y - compareY));
                if (v.y - compareY < -Block.nowBlock.curSize * 0.5f) // ?????? ????????? ????????? ??????.
                    v.y = compareY - Block.nowBlock.curSize;
                else
                    v.y = compareY;
            }
        }

        return v;

    }


    public static Vector2 ConvertTileVec(Vector2 v)
    {
        // if (!Block.nowBlock) return v;

        // v.x += (Block.nowBlock.curSize * (map_size.x - 1)) * 0.5f - CreateMap.instance.transform.position.x;
        // v.x += Block.nowBlock.curSize * 0.5f;

        // v.y = -v.y;
        // v.y += (Block.nowBlock.curSize * (map_size.y - 1)) * 0.5f + CreateMap.instance.transform.position.y;
        // v.y += Block.nowBlock.curSize * 0.5f;

        // return v;

        if (!Block.nowBlock) return v;

        v.x += (Block.nowBlock.curSize * (map_size.x - 1)) * 0.5f - CreateMap.instance.transform.position.x;

        if (v.x > 0)
            v.x += Block.nowBlock.curSize * 0.5f;
        else
            v.x -= Block.nowBlock.curSize * 0.5f;

        v.y = -v.y;
        v.y += (Block.nowBlock.curSize * (map_size.y - 1)) * 0.5f + CreateMap.instance.transform.position.y;
        if (v.y > 0)
            v.y += Block.nowBlock.curSize * 0.5f;
        else
            v.y -= Block.nowBlock.curSize * 0.5f;

        return v;
    }

    //????????? ????????? ????????? ????????? ????????? ??????
    public static int InTheBlockCount(Block block, List<Vector2> list = null)
    {
        int res = 0;

        List<Vector2> Set = new List<Vector2>();

        for (int r = 0; r < instance.createMap.map_size.y; r++)
            for (int c = 0; c < instance.createMap.map_size.x; c++)
                if (instance.createMap.MAP[c, r])
                    Set.Add(instance.createMap.transform.position + Tile[c, r].transform.localPosition);


        if (list == null)
            list = block.GetBlocksArray();

        for (int i = 0; i < Set.Count; i++)
            foreach (Vector2 vec in list)
                if (Set[i] == vec)
                    res++;

        //print("InTheBlockCount() tilePos in Set: " + Set[i].ToString("F4"));

        // foreach (Vector2 vec in list)
        //     if (Set.Contains(vec))
        //     {
        //         res++;
        //         print("InTheBlockCount() vec in list: " + vec.ToString("F3"));
        //     }
        //     else
        //     {
        //         print("InTheBlockCount() vec without list" + vec.ToString("F3"));
        //     }
        return res;
    }

    private void CheckTile()
    {
        List<Vector2> Set = new List<Vector2>();
        foreach (Block b in blockData)
            for (int c = 0; c < b.block_size; c++)
                for (int r = 0; r < b.block_size; r++)
                    if (b.MAP[c, r])
                    {
                        Vector2 mainPos = b.transform.position;
                        Vector2 temp = new Vector2(c, -r) * Block.nowBlock.curSize;
                        Set.Add(new Vector2(mainPos.x + temp.x, mainPos.y + temp.y));
                    }

        for (int c = 0; c < createMap.map_size.x; c++)
            for (int r = 0; r < createMap.map_size.y; r++)
                if (createMap.MAP[c, r])
                    Tile[c, r].SetIsFill(Set.Contains(createMap.transform.position + Tile[c, r].transform.localPosition));
                
                    
    }

    // ?????? ??? ?????? ????????? optional featrue??? ??????.
    // ???????????? ????????? ???, ??? ????????? ????????? ??? ??????????????? ??????.
    public void SettingCheck(bool isRelease)
    {
        int transedX = (int)(bv.x / Block.nowBlock.curSize);
        int transedY = (int)(bv.y / Block.nowBlock.curSize);

        bool[,] tileState = new bool[map_size.x, map_size.y];
        if (Block.nowBlock)
        {
            if (!isRelease)
            {
                for (int r = 0; r < map_size.y; r++)
                    for (int c = 0; c < map_size.x; c++)
                        Tile[c, r].SetIsFill(false);

                for (int r = Mathf.Max(0, transedY); r < Mathf.Min(createMap.map_size.y, transedY + Block.nowBlock.block_size); r++)
                    for (int c = Mathf.Max(0, transedX); c < Mathf.Min(createMap.map_size.x, transedX + Block.nowBlock.block_size); c++)
                    {
                        // ????????? ??????
                        int ac = c - transedX; // 0
                        int ar = r - transedY; // 2

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
                    print("inTheBlockCount" + inTheBlockCount); 
                    TouchChecker.isDragging = true;
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

    internal void CheckVictory()
    {
        for (int w = 0; w < Tile.GetLength(0); w++)
            for (int h = 0; h < Tile.GetLength(1); h++)
                if (createMap.MAP[w, h])
                    if (Tile[w, h].GetIsFill() == false)
                        return;

        VictoryCaller.InvokeWinEvent(true);
    }

    private void OnDisable()
    {
        CheckVictoryDele = null;
    }

    public static void InvokeCheckVictoryDele()
    {
        CheckVictoryDele?.Invoke();
    }
}

// public static Vector2 ConvertCeilVec(Vector2 v)
// {
//     if (GameManager.map_size.x % 2 == 1)
//         v.x += 0.5f;
//     v.x = Mathf.FloorToInt(v.x);
//     if (GameManager.map_size.x % 2 == 0)
//         v.x += 0.5f;

//     if (GameManager.map_size.y % 2 == 1)
//         v.y += 0.5f;
//     v.y = Mathf.FloorToInt(v.y);
//     if (GameManager.map_size.y % 2 == 0)
//         v.y += 0.5f;
//     return v;
// }

// public static Vector2 ConvertTileVec(Vector2 v)
// {
//     v.x += (CreateMap.OBJ_X * (map_size.x - 1)) * 0.5f - (int)CreateMap.instance.transform.position.x;
//     v.x += 0.5f;
//     v.x = Mathf.FloorToInt(v.x);

//     v.y = -v.y;
//     v.y += (CreateMap.OBJ_Y * (map_size.y - 1)) * 0.5f + (int)CreateMap.instance.transform.position.y;
//     v.y += 0.5f;
//     v.y = Mathf.FloorToInt(v.y);
//     return v;
// }
