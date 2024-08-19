using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Decorations");

        if (sprites.Length > 0)
        {
            Sprite randomSprite = sprites[Random.Range(0, sprites.Length)];
            spriteRenderer.sprite = randomSprite;
        }
        else
        {
            Debug.LogError("No sprites found in Resources/Sprites/Decorations!");
        }
    }
}
