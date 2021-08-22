using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerDataFromJson : MonoBehaviour
{
    public static PlayerDataFromJson instance;
    PlayerJson json;

    private void Awake() => instance = this;

    public PlayerJson Json
    {
        get { return json; }
        set
        {
            json = value;
        }
    }

    private void Start()
    {
        json = null;
        GetDataFromJson();
        SetData();
    }

    public void GetDataFromJson()
    {
        var obj = Resources.Load("PlayerInfo/PlayerData");

        json = JsonUtility.FromJson<PlayerJson>(obj.ToString());

        if (json != null)
        {
            print(JsonUtility.ToJson(json,true));
            print(json.HaveCats.Count);
        }
    }

    public void SetData()
    {
        Cat c = new Cat(200003, "에바냥이");
        json.HaveCats.Add(c);
        // JsonUtility.ToJson<PlayerJson>(json);
        string pJson = JsonUtility.ToJson(json,true);

        print(pJson);
        
        // string path = Resources.Load("PlayerInfo").ToString() + "PlayerData.json";
        print(Application.dataPath);
        string path = Path.Combine(Application.dataPath, "Resources/PlayerInfo/PlayerData.json");
        File.WriteAllText(path, pJson);
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
    public List<Cat> HaveCats;
    public List<Board> HaveBoards;
    public List<Food> HaveFoods;
    public List<BGM> HaveBGMs;
    public List<Background> HaveBackgrounds;
}


