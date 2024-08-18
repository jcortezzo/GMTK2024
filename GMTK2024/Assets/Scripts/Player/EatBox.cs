using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EatBox : MonoBehaviour
{
    public UnityEvent EatGroundEvent = new UnityEvent();
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.tag == "Ground")
        {
            EatGroundEvent.Invoke();
            Destroy(collision.gameObject);
        }
    }
}
