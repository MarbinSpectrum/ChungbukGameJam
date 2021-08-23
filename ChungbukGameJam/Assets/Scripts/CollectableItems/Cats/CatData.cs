using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CatData : ICollectable
{
    // Start is called before the first frame update
    public int CatID;
    public string CatName;

    public List<Food> favoriteFoods = new List<Food>();

    public CatData(int id, string name, List<Food> favor)
    {
        CatID = id;
        CatName = name;
        favoriteFoods = favor;
    }

    public void AddThisToPlayerData(PlayerDataFromJson playerDataFromJson)
    {
        // playerDataFromJson.Json.haveCats.
    }

    public void RemoveThisFromPlayerData(PlayerDataFromJson playerDataFromJson)
    {

    }
}
