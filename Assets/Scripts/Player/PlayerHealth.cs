using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public float maxHealth = 100;
    public float health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        healthBar.SetHealth(health);
    }

    public void Heal(float healing)
    {
        health = Mathf.Clamp(health + healing, 0, maxHealth);
        healthBar.SetHealth(health);
    }
}
