using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour{
    PlayerJson playerHaveData;

    public void GetItems()
    {
        PlayerDataFromJson.instance.GetDataFromJson();
        PlayerJson playerjson = PlayerDataFromJson.instance.Json;

        List<CatData> catDatas = playerjson.HaveCats;

        // for (int i = 0; i < playerjson.HaveCats.Count; i++)
        // {
        //     print(catDatas[i].itemName);
        // }
    }
}
