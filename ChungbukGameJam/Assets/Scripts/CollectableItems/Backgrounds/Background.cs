using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Background : Item
{
    public Background(string name, int idx, Sprite portrait) : base(name, idx, portrait)
    {
        itemName = name;
        itemIdx = idx;
        itemPortrait = portrait;
    }
}
