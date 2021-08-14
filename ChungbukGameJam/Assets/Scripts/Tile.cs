using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    private SpriteRenderer spriteRenderer
    {
        get
        {
            if (SpriteRenderer == null)
                SpriteRenderer = GetComponent<SpriteRenderer>();
            return SpriteRenderer;
        }
    }

    public void Normal()
    {
        spriteRenderer.color = Color.white;
    }

    public void Highight()
    {
        spriteRenderer.color = Color.gray;
    }
}
