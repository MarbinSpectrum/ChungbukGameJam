using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStore : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameManager gameManager;
    public List<Block> blocks = new List<Block>();

    [SerializeField]
    int startX;

    public static int boundSizeY;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        blocks = GameManager.instance.blockData;

        spriteRenderer = GetComponent<SpriteRenderer>();
        boundSizeY = (int)(-spriteRenderer.bounds.size.y * 0.5f + transform.position.y); // transform.position.y;
        print(boundSizeY);
        startX = (int)(spriteRenderer.bounds.size.x * -0.5f);

        if (blocks.Count > 0)
        {
            for (int b = 0; b < blocks.Count; b++)
            {
                DisplayPos dp = new DisplayPos(spriteRenderer, blocks.Count);

                int realX = blocks[b].MAP.GetLength(0);
                int realY = blocks[b].MAP.GetLength(1);
                //int realX = 0;

                for (int i = 0; i < blocks[b].MAP.GetLength(0); i++)
                {
                    int tempX = 0;

                    for (int j = 0; j < blocks[b].MAP.GetLength(1); j++)
                        if (blocks[b].MAP[i, j])
                            tempX++;

                    if (realX <= tempX)
                        realX = tempX;
                }

                // int x = dp.GetDisplayPos(b, blocks[b].MAP.GetLength(0));
                int x = dp.GetDisplayPosX(b, realX) + startX;
                int y = (int)(spriteRenderer.gameObject.transform.position.y + realY * 0.1f);

                blocks[b].transform.position = GameManager.ConvertCeilVec(new Vector2(x, y));


                // blocks[b].gameObject.transform.position = new Vector3Int(x + startX, y, 0);
                // print(blocks[b].gameObject.transform.position);
            }
        }
    }
}

public struct DisplayPos
{
    public SpriteRenderer spriteRect;
    public int count;

    public DisplayPos(SpriteRenderer spriteRect, int count)
    {
        this.spriteRect = spriteRect;
        this.count = count;
    }

    public int GetDisplayPosX(int idx, int blockX)
    {
        int spacing = 0;

        if (blockX < 2)
            spacing = 1;

        int alignX = (int)(spriteRect.bounds.size.x / count) * idx + spacing;// + (int)(blockX * 0.5f);
        return alignX;
    }

    // public (int, int) GetDisplayPos(int idx, int blockX)
    // {
    //     int alignX = (int)(spriteRect.bounds.size.x / count) * idx + blockX;
    //     int alignY = (int)spriteRect.gameObject.transform.position.y;
    //     // int alignY = (int)(spriteRect.bounds.size.y / count) * idx;
    //     return (alignX, alignY);
    // }
}
