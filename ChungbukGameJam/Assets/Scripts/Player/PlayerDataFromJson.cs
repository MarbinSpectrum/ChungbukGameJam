using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerDataFromJson : MonoBehaviour
{
    public static PlayerDataFromJson instance;
    PlayerJson json;

    public PlayerJson Json
    {
        get { return json; }
        set
        {
            json = value;
        }
    }

    private void Awake()
    {
        instance = this;
        json = null;
        // SetData();
    }

    public void GetDataFromJson()
    {
        var obj = Resources.Load("PlayerInfo/PlayerData");

        Json = JsonUtility.FromJson<PlayerJson>(obj.ToString());

        // if (json != null)
        // {
        //     print(json);
        //     print(JsonUtility.ToJson(json,true));
        //     print(json.HaveCats.Count);
        // }
    }

    public void SetData()
    {
        // CatData c = new CatData(200003, "에바냥이", null);
        // json.HaveCats.Add(c);
        // string pJson = JsonUtility.ToJson(json,true);

        // print(pJson);
        
        // string path = Resources.Load("PlayerInfo").ToString() + "PlayerData.json";
        // print(Application.dataPath);
        // string path = Path.Combine(Application.dataPath, "Resources/PlayerInfo/PlayerData.json");
        // File.WriteAllText(path, pJson);
    }

    public PlayerJson GetItems()
    {
        GetDataFromJson();
        List<CatData> catDatas = Json.HaveCats;

        for (int i = 0; i < Json.HaveCats.Count; i++)
            print(catDatas[i].itemName);

        return Json;
    }

    public void SaveDataToJson()
    {

    }
}

[System.Serializable]
public class PlayerJson
{
    public int PlayerID;
    public string PlayerName;

    public int HaveGold;

    [SerializeField]
    public List<CatData> HaveCats;
    public List<Board> HaveBoards;
    public List<FoodData> HaveFoods;
    public List<BGM> HaveBGMs;
    public List<Background> HaveBackgrounds;
}


