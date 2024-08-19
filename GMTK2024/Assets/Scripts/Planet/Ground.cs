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
                    collider.size = spriteRenderer.bounds.size;
                    collider.direction = spriteRenderer.bounds.size.x > spriteRenderer.bounds.size.y
                        ? CapsuleDirection2D.Horizontal
                        : CapsuleDirection2D.Vertical;
                }
                Destroy(child.gameObject, 10f);
            }
        }
    }
}
