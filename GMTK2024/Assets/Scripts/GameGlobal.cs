using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "GameGlobal", menuName = "ScriptableObject/GameGlobal")]
public class GameGlobal : ScriptableObject
{
    public bool GoreEnable;
    public int GroundInUniverse;
    public void OnEnable()
    {
        GoreEnable = false;
        GroundInUniverse = 0;
    }
}
