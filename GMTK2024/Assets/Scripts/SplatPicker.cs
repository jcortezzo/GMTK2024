using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatPicker : MonoBehaviour
{
    [SerializeField] private GameGlobal gameGlobal;
    [SerializeField] private Sprite RatedRSpat;
    private SpriteRenderer sr;
    void Start()
    {
        if(gameGlobal.GoreEnable)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = RatedRSpat;
        }
    }
}
