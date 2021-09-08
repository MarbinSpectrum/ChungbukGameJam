using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int stageIdx = 0;
    public StageData stageData;
    public BlockStore blockStore;

    public void InstantiateBlocks()
    {
        blockStore = FindObjectOfType<BlockStore>();

        if (stageData)
            for (int i = 0; i < stageData.blocks.Count; i++)
                for (int j = 0; j < stageData.blocks[i].num; j++)
                {
                    GameObject blockObj = Instantiate(stageData.blocks[i].block).gameObject;
                    blockObj.transform.SetParent(blockStore.transform);
                }
    }

    // public void SetTileMapToCreateMap()
    // {
    //     if (stageData)
    //     {
    //         CreateMap.instance.map_size = stageData.map_size;
    //         CreateMap.instance.MAP = stageData.MAP;
    //     }
    // }
}
