using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [field:SerializeField]
    public int Health { get; private set; }
    [field: SerializeField]
    public int GroundEaten { get; private set; }

    public void Damage(int amount)
    {
        Health -= amount;
        Health = Health < 0 ? 0 : Health;
    }

    public void EatGround()
    {
        GroundEaten++;
    }
}
