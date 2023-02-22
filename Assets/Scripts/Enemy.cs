using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header ("Enemy Stats")]
    public int MaxHealth;
    public int Attack;
    public int Defense;
    public int OnDeath_EXP;
    public int OnDeath_Money;

    private Slider EnemyHPSlider;
    private PlayerMainScript MainScript;

    public void Awake()
    {
        EnemyHPSlider = MainScript.CombatCanvas.transform.GetChild(0).GetComponent<Slider>();

        EnemyHPSlider.maxValue = MaxHealth;
        SetHealth((int)EnemyHPSlider.maxValue);
    }

    public void SetHealth(int Health)
    {
        EnemyHPSlider.value = Health;
        if (EnemyHPSlider.value <= 0) OnDeath();
    }

    private void OnDeath()
    {

    }
}
