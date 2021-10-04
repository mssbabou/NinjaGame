using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public float MaxHealth = 100;
    public float Health;
    
    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        healthBar.SetHealth(Health);
    }

    public void Heal(float healing)
    {
        Health += Mathf.Clamp(healing, 0, 100);
        healthBar.SetHealth(Health);
    }
}
