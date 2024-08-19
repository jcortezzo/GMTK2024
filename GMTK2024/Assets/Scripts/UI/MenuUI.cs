using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameGlobal gameGlobal;
    [SerializeField] Toggle goreToggle;

    // Update is called once per frame
    void Update()
    {
        gameGlobal.GoreEnable = goreToggle.isOn;
    }

    public void StartGame(string gameScene)
    {
        SceneManager.LoadScene(gameScene);
    }
}
