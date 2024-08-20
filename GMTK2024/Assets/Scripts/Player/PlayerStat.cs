using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerStat : MonoBehaviour
{
    [field: SerializeField]
    public int Health { get; private set; }
    [field: SerializeField]
    public int GroundEaten { get; private set; }
    [field: SerializeField]
    public int Level { get; private set; }

    public int MAX_LEVEL = 10;
    public int[] GroundForNextLevel = { 30, 150, 300, 500, 1000, 2000, 3000, 4000, 5000, 6000 };
    public int[] CameraLevelUpScale = { 16, 14, 12, 10, 8, 6, 5, 4, 3, 3 };
    public int[] MinJumpScale = { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    public int[] MaxJumpScale = { 20, 25, 30, 35, 40, 50, 55, 60, 65, 70 };
    public int[] FontSizeMin = { 40, 40, 40, 60, 60, 60, 70, 70, 70, 70 };
    public int[] FontSizeMax = { 60, 60, 60, 80, 80, 80, 80, 80, 80, 80 };

    private PixelPerfectCamera ppCamera;
    private PlayerJump playerJump;
    private TextSpawner playerTextSpawner;

    private void Start()
    {
        Level = 1;
        ppCamera = Camera.main.gameObject.GetComponent<PixelPerfectCamera>();
        playerJump = GetComponent<PlayerJump>();
        playerTextSpawner = GetComponent<TextSpawner>();

    }
    public void Damage(int amount)
    {
        Health -= amount;
        Health = Health < 0 ? 0 : Health;
    }

    public void EatGround()
    {
        GroundEaten++;
    }

    private void Update()
    {
        var nextLevelGroundCount = GetNumberOfGroundForNextLevel();
        if (nextLevelGroundCount == -1) return;
        if (GroundEaten >= nextLevelGroundCount && Level - 1 < MAX_LEVEL)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        LevelUpScale();
        LevelUpCamera();
        LevelUpJump();
        LevelUpText();
    }
    private void LevelUpScale()
    {
        var mmIcon = transform.Find("MinimapIcon");
        mmIcon.parent = null;
        transform.localScale = new Vector3(Level, Level, Level);
        mmIcon.parent = this.transform;
    }
    private void LevelUpCamera()
    {
        if (Level >= MAX_LEVEL) return;
        var scale = CameraLevelUpScale[Level];
        DOTween.To(() => ppCamera.assetsPPU, x => ppCamera.assetsPPU = x, scale, 0.5f);
    }

    private void LevelUpJump()
    {
        if (Level >= MAX_LEVEL) return;
        var minJumpVal = MinJumpScale[Level];
        var maxJumpVal = MaxJumpScale[Level];
        playerJump.MinJumpForce = minJumpVal;
        playerJump.MaxJumpForce = maxJumpVal;

    }

    private void LevelUpText()
    {
        if (Level >= MAX_LEVEL) return;
        playerTextSpawner.MinTextSize = FontSizeMin[Level];
        playerTextSpawner.MaxTextSize = FontSizeMax[Level];
    }
    private int GetNumberOfGroundForNextLevel()
    {
        if (Level >= MAX_LEVEL) return -1;
        var x = Level - 1;
        return GroundForNextLevel[x];
    }
}
