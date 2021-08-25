using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Food: MonoBehaviour
{
    Block block;

    public Sprite sprite;
    public int foodGaugeNum;

    // Start is called before the first frame update
    void Start()
    {
        block = GetComponent<Block>();
        
    }

    public void RevealSprite()
    {
        block.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
