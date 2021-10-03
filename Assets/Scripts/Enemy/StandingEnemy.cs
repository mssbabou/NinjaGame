using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StandingEnemy : MonoBehaviour
{
    float attackCooldown = 1;
    float currentCooldown = 0;
    
    private void Update()
    {
        if (currentCooldown >= 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        PlayerCombat PC = hitInfo.GetComponent<PlayerCombat>();
        if (PC != null && currentCooldown <= 0)
        {
            PC.TakeDamage(10);
            currentCooldown = attackCooldown;
        }
    }
}
