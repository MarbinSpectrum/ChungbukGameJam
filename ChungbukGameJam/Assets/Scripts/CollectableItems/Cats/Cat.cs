using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cat : MonoBehaviour
{
    // Start is called before the first frame update
    public int CatID;
    public string CatName;

    public SpriteRenderer spriteRenderer;

    public List<Food> favoriteFoods = new List<Food>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Cat(int id, string name, Sprite sprite, List<Food> favorFood)
    {
        CatID = id;
        CatName = name;
        spriteRenderer.sprite = sprite;
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
