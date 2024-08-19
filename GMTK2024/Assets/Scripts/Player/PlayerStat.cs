using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [field: SerializeField]
    public int Health { get; private set; }
    [field: SerializeField]
    public int GroundEaten { get; private set; }
    [field: SerializeField]
    public int Level { get; private set; }
    private void Start()
    {
        Level = 1;
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
        if (GroundEaten >= nextLevelGroundCount)
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
        var x = Level;
        // y = mx + b
        int b = 20;
        int m = 10;
        var y = m * Level + b;

        // y = 60^2 - 100 + 60
        return 60 * x * x - 100 * x + 60;
    }
}
