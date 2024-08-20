using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGameUI : MonoBehaviour
{
    public PlayerStat playerStat;
    public GameGlobal gameGlobal;
    
    public GameObject deadPanel;
    public GameObject winPanel;
    public TMP_Text planetConsumedText;

    private float elaspedTime;
    public bool universeConsumed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!universeConsumed) elaspedTime += Time.deltaTime;
        if (playerStat.IsDead)
        {
            deadPanel.SetActive(true);
            winPanel.SetActive(false);
        } else if(UniverseConsumedPercentage() >= 80)
        {
            universeConsumed = true;
            winPanel.SetActive(true);
            planetConsumedText.text = $"You consumed 80% of planet in {Mathf.FloorToInt(elaspedTime)} seconds";
        }
    }

    public float UniverseConsumedPercentage()
    {
        if (gameGlobal.GroundInUniverse == 0) return -1;
        return ((float)playerStat.GroundEaten / gameGlobal.GroundInUniverse) * 100;
    }

    public void RestartLevel()
    {
        gameGlobal.ResestVariable();
        winPanel.SetActive(false);
        deadPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
