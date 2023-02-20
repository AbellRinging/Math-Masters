using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Parent_PlayerScript
{
    public int StartingMaxHealth;
    private Slider slider;

    protected override void Custom_Start()
    {
        slider = MainScript.EssentialCanvas.transform.GetChild(0).GetComponent<Slider>();

        SetMaxHealth(StartingMaxHealth);
        SetHealth((int)slider.maxValue);
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0;
    }

    public void SetHealth(int Health)
    {
        slider.value = Health;
    }
}
