using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string itemName;
    public int itemIdx;
    public Sprite itemPortrait;

    // public string ItemName
    // {
    //     get
    //     {
    //         return itemName;
    //     }

    //     set
    //     {
    //         if (value == null)
    //             value = "Default" + this.GetType().ToString();
    //     }
    // }

    // public int ItemIdx { get; set; }
    // public Sprite ItemPortrait { get; set; }

    public Item()
    {

    }

    public Item(string name, int idx, Sprite portrait)
    {
        itemName = name;
        itemIdx = idx;
        itemPortrait = portrait;
    }
}
