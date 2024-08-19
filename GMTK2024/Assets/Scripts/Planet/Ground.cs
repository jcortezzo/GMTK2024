using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnDestroy()
    {
        // Check if the ground tile has any children (decorations)
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Decoration>() == null)
                {
                    continue;
                }

                child.parent = null;
                Rigidbody2D rb = child.gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.mass = 0.1f;
                CapsuleCollider2D collider = child.gameObject.AddComponent<CapsuleCollider2D>();
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
                    collider.size = spriteSize * 0.9f; // Scale down slightly to fit better
                    collider.offset = new Vector2(0, spriteSize.y * 0.1f); // Adjust the offset if needed
                    collider.direction = spriteRenderer.bounds.size.x > spriteRenderer.bounds.size.y
                        ? CapsuleDirection2D.Horizontal
                        : CapsuleDirection2D.Vertical;
                }
                Destroy(child.gameObject, 10f);
            }
        }
    }

    //void OnBecameInvisible()
    //{
    //    gameObject.SetActive(false);
    //}

    //void OnBecameVisible()
    //{
    //    gameObject.SetActive(true);
    //}

}
