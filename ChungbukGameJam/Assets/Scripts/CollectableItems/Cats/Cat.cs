using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cat : MonoBehaviour
{
    // Start is called before the first frame update
    public int CatID;
    public string CatName;
    
    public List<Food> favoriteFoods = new List<Food>();

    public Cat(int id, string name, List<Food> favorFood)
    {
        CatID = id;
        CatName = name;
        favoriteFoods = favorFood;
    }

    public void AddThisToPlayerData(PlayerDataFromJson playerDataFromJson)
    {
        // playerDataFromJson.Json.haveCats.
    }

    public static void RemoveThisFromPlayerData(PlayerDataFromJson playerDataFromJson)
    {

    }
}
