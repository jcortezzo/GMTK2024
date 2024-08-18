using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerStat _playerStat;

    [SerializeField] private TMP_Text _healthTMP;
    [SerializeField] private TMP_Text _groundText;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _healthTMP.text = $"Health: {_playerStat.Health}";
        _groundText.text = $"Ground: {_playerStat.GroundEaten}";

    }
}
