using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerDataFromJson playerData;

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
        cats.Clear();
        SetCatsFromJson();
    } 

    void SetCatsFromJson()
    {
        for (int i = 0; i < playerData.Json.HaveCats.Count; i++)
        {
            CatData catData = new CatData(playerData.Json.HaveCats[i].CatID, playerData.Json.HaveCats[i].CatName, playerData.Json.HaveCats[i].favoriteFoods);
            cats.Add(catData);
        } 
    }
    
    
}

