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
        tiles = new Tile[cellX, cellY];

        stageManager = FindObjectOfType<StageManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        boundSizeY = (cellY * Block.shrinkRate) * 0.5f;
    }

    public void CreateOutline()
    {
        CreateBasicTile();

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

    private void CreateBasicTile()
    {
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
    }
}