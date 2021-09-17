using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStoreTileMap : MonoBehaviour
{
    public static Vector2 tileStorePos;

    public int cellX, cellY;

    public Tile basicTile;
    public Tile[,] tiles;

    public static float boundSizeY;

    public Transform tileStore;
    StageManager stageManager;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
         tileStorePos = transform.position;
    }

    private void Start()
    {
        tiles = new Tile[cellX, cellY];

        stageManager = FindObjectOfType<StageManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        boundSizeY = (cellY * Block.shrinkRate) * 0.5f; // transform.position.y;

        for (int i = 0; i < cellX; i++)
        {
            for (int j = 0; j < cellY; j++)
            {
                Tile tempTile = Instantiate(basicTile);
                tempTile.transform.localScale *= Block.shrinkRate;
                tempTile.transform.position = new Vector3(i * Block.shrinkRate, j * Block.shrinkRate, 0) + transform.position;
                tempTile.transform.SetParent(tileStore);
                tempTile.gameObject.SetActive(true);
                tiles[i, j] = tempTile;
            }
        }        

        CreateOutline();
        AllocatePosToBlock();
    }

    public void CreateOutline()
    {
        for (int tileMapX = 0; tileMapX < tiles.GetLength(0); tileMapX++)
        {
            if (tiles[tileMapX, 0])
            {
                tiles[tileMapX, 0].GetComponent<SpriteRenderer>().color = Color.black;
                tiles[tileMapX, 0].SetIsFill(true);
            }

            if (tiles[tileMapX, tiles.GetLength(1) - 1])
            {
                tiles[tileMapX, tiles.GetLength(1) - 1].GetComponent<SpriteRenderer>().color = Color.black;
                tiles[tileMapX, tiles.GetLength(1) - 1].SetIsFill(true);
            }
        }

        for (int tileMapY = 0; tileMapY < tiles.GetLength(1); tileMapY++)
        {
            if (tiles[0, tileMapY])
            {
                tiles[0, tileMapY].GetComponent<SpriteRenderer>().color = Color.black;
                tiles[0, tileMapY].SetIsFill(true);
            }
            if (tiles[tiles.GetLength(0) - 1, tileMapY])
            {
                tiles[tiles.GetLength(0) - 1, tileMapY].GetComponent<SpriteRenderer>().color = Color.black;
                tiles[tiles.GetLength(0) - 1, tileMapY].SetIsFill(true);
            }
        }
    }

    public void AllocatePosToBlock()
    {
        int pointX = 0;
        int pointY = 0;

        int idxX = 0, idxY = 0;
        Vector3 startPos = transform.position;

        Block[,] landedBlocks = new Block[stageManager.stageData.blocks.Count, stageManager.stageData.blocks.Count];

        // 높이 검증, 가로 검증을 진행해야 함.

        for (int b = 0; b < GameManager.instance.blockData.Count; b++)
        {
            Block block = GameManager.instance.blockData[b];
            block.transform.position = Vector3.zero;

            pointY = 0;

            // 다음 블록이 수용 공간에서 벗어날 경우, 위로 블록을 보냄.
            // 양옆의 테두리 칸(1)을 빼기 위해서 cellX -2를 해줌
            if (cellX - 2 < pointX + block.GetBlockRealSize().Item1)
            {
                idxX = 0;
                idxY++;
                pointX = 0;
            }

            // 얼마나 위로 올라가야하는 지 계산.
            // pointX, pointY에 +1을 해주는 이유: 이격 거리 생성.
            if (idxY > 0)
            {
                for (int y = 0; y < idxY; y++)
                    pointY += landedBlocks[idxX, y].GetBlockRealSize().Item2 + 1;
                pointY += block.GetBlockRealSize().Item2 - 1;
            }
            else
                pointY = block.GetBlockRealSize().Item2 - 1;

            Vector2 tempPos = (Vector2)tiles[1, 1].transform.position;
            tempPos += new Vector2(pointX, pointY) * block.curSize;

            pointX += block.GetBlockRealSize().Item1 + 1;

            block.transform.position = tempPos;
            block.SetBasePos(tempPos);
            landedBlocks[idxX, idxY] = block;

            // print(block.name + ": " + "landedBlocks[" + idxX+"," + idxY+ "]");

            idxX++;
        }
    }
}