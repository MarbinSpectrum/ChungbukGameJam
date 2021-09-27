using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CatData : Item, ICollectable
{
    public List<FoodData> favoriteFoods = new List<FoodData>();

    public CatData(string name, int idx, Sprite portrait, List<FoodData> favoriteFoods): base(name, idx, portrait)
    {
        itemName = name;
        itemIdx = idx;
        itemPortrait = portrait;

        this.favoriteFoods = favoriteFoods;
    }

    public void AddThisToPlayerData(PlayerDataFromJson playerDataFromJson)
    {
        // playerDataFromJson.Json.haveCats.
    }

    public void RemoveThisFromPlayerData(PlayerDataFromJson playerDataFromJson)
    {

    }

    public void SetFavorFoods(List<FoodData> favor)
    {
        favoriteFoods = favor;
    }

    public List<FoodData> GetFavorFoods()
    {
        return favoriteFoods;
    }
}
