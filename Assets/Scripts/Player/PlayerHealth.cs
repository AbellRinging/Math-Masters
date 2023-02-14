using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Parent_PlayerScript
{

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    
    protected override void Custom_Start()
    {

    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     currentHealth = maxHealth;
    //     healthBar.SetMaxHealth(maxHealth);
    // }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
