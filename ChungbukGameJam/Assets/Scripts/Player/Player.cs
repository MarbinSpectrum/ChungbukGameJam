using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerDataFromJson playerData;
    public Inventory inventory;

    public List<CatData> cats = new List<CatData>();

    private void Awake()
    {
        if (instance)
            Destroy(instance);
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        playerData = GetComponent<PlayerDataFromJson>();
        inventory = GetComponent<Inventory>();

        cats.Clear();
        inventory.GetItems();
        //SetCatsFromJson();
        // inventory.GetItems();        
    } 

    void SetCatsFromJson()
    {
        for (int i = 0; i < playerData.Json.HaveCats.Count; i++)
        {
            CatData catData = playerData.Json.HaveCats[i];
            // CatData catData = new CatData(playerData.Json.HaveCats[i].ItemName, playerData.Json.HaveCats[i].ItemIdx, playerData.Json.HaveCats[i].ItemPortrait, playerData.Json.HaveCats[i].GetFavorFoods());
            cats.Add(catData);
        } 
    }
    
    
}

