using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatBox : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.tag == "Ground")
        {
            Destroy(collision.gameObject);
        }
    }
}
