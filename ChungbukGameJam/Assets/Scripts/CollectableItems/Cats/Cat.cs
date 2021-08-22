using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cat : ICollectable
{
    // Start is called before the first frame update
    public int CatID;
    public string CatName;
    
    public List<Food> favoriteFoods = new List<Food>();

    public Cat(int id, string name)
    {
        CatID = id;
        CatName = name;
    }

    public void AddThisToPlayerData(PlayerDataFromJson playerDataFromJson)
    {
        // playerDataFromJson.Json.haveCats.
    }

    public void RemoveThisFromPlayerData(PlayerDataFromJson playerDataFromJson)
    {

    }
}
