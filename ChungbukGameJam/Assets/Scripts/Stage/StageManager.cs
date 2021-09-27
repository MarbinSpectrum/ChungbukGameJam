using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageData stageData;
    public BlockStore blockStore;
    public BlockStoreTileMap blockStoreTileMap;

    private void Start()
    {
        blockStoreTileMap.CreateOutline();
        AllocatePosToBlock();
        // InstantiateBlocks();
    }

    public void AllocatePosToBlock()
    {
        int pointX = 0;
        int pointY = 0;

        int idxX = 0, idxY = 0;
        Vector2 tempPos = (Vector2)blockStoreTileMap.tiles[1, 1].transform.position;

        int blockRealCount = 0;
        for (int i = 0; i < stageData.blocks.Count; i++)
        {
            blockRealCount += stageData.blocks[i].num;
        }

        Block[,] landedBlocks = new Block[blockRealCount, blockRealCount];

        // 높이 검증, 가로 검증을 진행해야 함.
        for (int b = 0; b < stageData.blocks.Count; b++)
        {
            for (int bc = 0; bc < stageData.blocks[b].num; bc++)
            {
                Block block = Instantiate(stageData.blocks[b].block);
                block.transform.position = Vector3.zero;
                block.transform.SetParent(blockStore.transform);

                // 다음 블록이 수용 공간에서 벗어날 경우, 위로 블록을 보냄.
                // 양옆의 테두리 칸(1)을 빼기 위해서 cellX -2를 해줌
                if (blockStoreTileMap.cellX - 2 < pointX + block.GetBlockRealSize().Item1)
                {
                    idxX = 0;
                    idxY++;
                    pointX = 0;
                }

                // 얼마나 위로 올라가야하는 지 계산.
                // pointX, pointY에 +1을 해주는 이유: 이격 거리 생성.
                if (idxY > 0)
                    for (int y = 0; y < idxY; y++)
                        pointY += landedBlocks[idxX, y].GetBlockRealSize().Item2 + 1;
                
                pointY += block.GetBlockRealSize().Item2 - 1;

                block.transform.position = tempPos + new Vector2(pointX, pointY) * Block.shrinkRate;
                block.SetBasePos(block.transform.position);
                landedBlocks[idxX, idxY] = block;

                pointX += block.GetBlockRealSize().Item1 + 1;
                idxX++;
                
                pointY = 0;
            }
        }
    }

    // public void InstantiateBlocks()
    // {
    //     blockStore = FindObjectOfType<BlockStore>();

    //     if (stageData)
    //         for (int i = 0; i < stageData.blocks.Count; i++)
    //             for (int j = 0; j < stageData.blocks[i].num; j++)
    //             {
    //                 GameObject blockObj = Instantiate(stageData.blocks[i].block).gameObject;
    //                 blockObj.transform.SetParent(blockStore.transform);
    //             }
    // }
}
