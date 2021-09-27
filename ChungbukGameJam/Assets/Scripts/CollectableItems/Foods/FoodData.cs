using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData: Item, ICollectable
{
    public FoodData(string name, int idx, Sprite portrait): base(name, idx, portrait)
    {
        itemName = name;
        itemIdx = idx;
        itemPortrait = portrait;
    }

    public void AddThisToPlayerData(PlayerDataFromJson playerDataFromJson)
    {
        // playerDataFromJson.Json.haveCats.
    }

    public void RemoveThisFromPlayerData(PlayerDataFromJson playerDataFromJson)
    {

    }
}
