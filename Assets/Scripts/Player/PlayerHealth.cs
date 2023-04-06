using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Parent_PlayerScript
{
    public int MaxHealth;
    private HeartContainerScript HeartContainer;

    protected override void Custom_Start()
    {
        HeartContainer = MainScript.EssentialCanvas.transform.Find("Player").transform.Find("Heart Container").GetComponent<HeartContainerScript>();
        HeartContainer.SpawnHearts(MaxHealth, true);
    }


    public bool TakeDamage()
    {
        return HeartContainer.ReduceHealth();
    }

    public void Heal(int HealthRegeneration)
    {
        for(int i = 0; i < HealthRegeneration; i++)
        {
            HeartContainer.RegenerateHealth();
        }
    }
}
