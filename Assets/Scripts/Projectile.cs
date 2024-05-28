using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float Damage
    {
        get;
        set;
    }

    private void Awake()
    {
        Destroy(gameObject, 30);
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent(out PlayerStats playerStats);
        if(!playerStats) return;
        playerStats.TakeDamage(Damage);
        Destroy(gameObject);
    }
}
