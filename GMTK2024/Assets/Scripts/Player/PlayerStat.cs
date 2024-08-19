using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    
    public int MAX_LEVEL = 6;
    public int[] GroundForNextLevel = { 20, 50, 100, 200, 500, 1000 };
    public int[] CameraLevelUpScale = { 16, 14, 12, 10, 8, 6 };
    public int[] MinJumpScale = { 5, 6, 7, 8, 9, 10 };
    public int[] MaxJumpScale = { 20, 25, 30, 35, 40, 50 };

    private PixelPerfectCamera ppCamera;
    private PlayerJump playerJump;

    private void Start()
    {
        Level = 1;
        ppCamera = Camera.main.gameObject.GetComponent<PixelPerfectCamera>();
        playerJump = GetComponent<PlayerJump>();
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
    }
    private void LevelUpScale()
    {
        transform.localScale = new Vector3(Level, Level, Level);
    }
    private void LevelUpCamera()
    {
        var scale = CameraLevelUpScale[Level];
        DOTween.To(() => ppCamera.assetsPPU, x => ppCamera.assetsPPU = x, scale, 0.5f);
    }

    private void LevelUpJump()
    {
        var minJumpVal = MinJumpScale[Level];
        var maxJumpVal = MaxJumpScale[Level];
        playerJump.MinJumpForce = minJumpVal;
        playerJump.MaxJumpForce= maxJumpVal;

    }
    private int GetNumberOfGroundForNextLevel()
    {
        var x = Level - 1;
        return GroundForNextLevel[x];
    }
}
