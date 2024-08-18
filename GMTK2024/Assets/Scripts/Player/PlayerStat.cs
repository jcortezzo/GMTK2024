using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [field:SerializeField]
    public int Health { get; private set; }
    [field: SerializeField]
    public int GroundEaten { get; private set; }
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
        
    }

    private int GetNumberOfGroundForNextLevel()
    {
        // y = mx + b
        int b = 20;
        int m = 2;
        return m * Level + b;
    }
}
