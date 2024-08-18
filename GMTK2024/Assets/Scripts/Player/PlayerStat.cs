using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [field:SerializeField]
    public int Health { get; private set; }
    [field: SerializeField]
    public int GroundEaten { get; private set; }
    [field: SerializeField]
    public int Level { get; private set; }
    private void Start()
    {
        Level = 0;
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
        if(GroundEaten >= nextLevelGroundCount)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        transform.localScale = new Vector3(Level, Level, Level);
    }
    private int GetNumberOfGroundForNextLevel()
    {
        // y = mx + b
        int b = 20;
        int m = 10;
        return m * Level + b;
    }
}
