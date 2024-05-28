using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health;


    public virtual void TakeDamage(float damageToTake)
    {
        if(health <= 0) return;
        health -= damageToTake;
    }
}
   