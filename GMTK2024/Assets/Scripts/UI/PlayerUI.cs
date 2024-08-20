using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameGlobal _gameGlobal;
    [SerializeField] private PlayerStat _playerStat;
    [SerializeField] private TMP_Text _healthTMP;
    [SerializeField] private TMP_Text _groundText;
    [SerializeField] private TMP_Text _universeConsumed;
    [SerializeField] private Image _universeFill;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _healthTMP.text = $"Health: {_playerStat.Health}";
        _groundText.text = $"Ground: {_playerStat.GroundEaten}";
        var consumePercentage = ((float)_playerStat.GroundEaten / _gameGlobal.GroundInUniverse) * 100;
        _universeConsumed.text = $"Universe Consumed {consumePercentage.ToString("0.00")}% ({_gameGlobal.GroundInUniverse})";
        _universeFill.fillAmount = (float)(100 - consumePercentage) / 100f;
    }
}
