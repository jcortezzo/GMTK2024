using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJumpUI : MonoBehaviour
{
    [SerializeField] private Image arrowUI;
    [SerializeField] private Image arrowOutline;
    private PlayerJump playerJump;
    // Start is called before the first frame update
    void Start()
    {
        playerJump = GetComponentInParent<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerJump != null)
        {
            arrowUI.fillAmount = playerJump.GetJumpSquatPercentage();
            arrowOutline.fillAmount = playerJump.GetJumpSquatPercentage() > 0 ? 1 : 0;
        }
    }
}
