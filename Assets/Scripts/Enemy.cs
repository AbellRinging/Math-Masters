using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header ("Enemy Stats")]
        [Tooltip ("The amount of Health it starts with. This number does not change during runtime")]
        public int MaxHealth;

        [Tooltip ("The amount of damage it will deal to the player on each turn")]
        public int Attack;

        [Tooltip ("The amount of damage reduced from the player")]
        public int Defense;
    
    [Header ("Rewards upon Death")]
        [Tooltip ("The amount of EXP rewarded to the player")]
        public int OnDeath_EXP;

        [Tooltip ("The amount of Money rewarded to the player")]
        public int OnDeath_Money;

    private Slider EnemyHPSlider;
    private PlayerMainScript MainScript;

    public void InitializeEnemy(PlayerMainScript PMS)
    {
        MainScript = PMS;
        EnemyHPSlider = MainScript.CombatCanvas.transform.GetChild(0).GetComponent<Slider>();
    }

    // Use this method in Player Combat???
    public void PrepareForCombat()
    {
        EnemyHPSlider.maxValue = MaxHealth;
        EnemyHPSlider.value = MaxHealth;
    }

    public void TakeDamage(int Damage)
    {
        int resultingDamage = Damage - Defense;
        if (resultingDamage > 0)
        {
            EnemyHPSlider.value -= resultingDamage;
            if (EnemyHPSlider.value <= 0) OnDeath();
        }
        else 
        {
            // Possibly make a "Blocked" thing appear?
        }

    }

    private void OnDeath()
    {
        // SOME STUFF IS NEEDED FIRST

        MainScript.MoneyScript.Set_Money(OnDeath_Money);

        MainScript.CombatScript.NextEnemyFight();

        Destroy(transform.gameObject);
    }
}
