using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Button> sortingButtons = new List<Button>();
    public List<Button> scrollviewButtons = new List<Button>();
    public GameObject itemArea;

    PlayerDataFromJson playerData;

    private void Start()
    {
        sortingButtons.Clear();
        scrollviewButtons.Clear();

        itemArea = GameObject.Find("ItemArea");

        sortingButtons.AddRange(GameObject.Find("SortingArea").GetComponentsInChildren<Button>());
        scrollviewButtons.AddRange(itemArea.GetComponentsInChildren<Button>());

        playerData = FindObjectOfType<PlayerDataFromJson>();

        SetUp("HaveCats");
    }

    public void SetUp(string sortBy)
    {
        PlayerJson pj = playerData.GetItems();

        for(int i = 0; i < scrollviewButtons.Count; i++)
            if( i < pj.HaveCats.Count)
            {
                print(i);
                scrollviewButtons[i].GetComponentInChildren<Text>().text = pj.HaveCats[i].itemName;
            }
                
        
    }
}
