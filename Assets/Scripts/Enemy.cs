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

        [Tooltip ("How difficult will the questions be")]
        public int Tier;
    
    [Header ("Rewards upon Death")]
        [Tooltip ("The amount of EXP rewarded to the player")]
        public int OnDeath_EXP;

        [Tooltip ("The amount of Money rewarded to the player")]
        public int OnDeath_Money;

    private Slider EnemyHPSlider;
    private PlayerMainScript MainScript;

    [HideInInspector] public bool AboutToDie = false;

    public void InitializeEnemy(PlayerMainScript PMS)
    {
        MainScript = PMS;
        EnemyHPSlider = MainScript.CombatCanvas.transform.GetChild(0).GetChild(1).GetComponent<Slider>();
    }

    public void PrepareForCombat()
    {
        EnemyHPSlider.maxValue = MaxHealth;
        EnemyHPSlider.value = MaxHealth;
    }

    public void TakeDamage(int Damage)
    {
        int resultingDamage = Damage - Defense;
            /* Sleep here for animation purposes? */
        if (resultingDamage > 0)
        {
            EnemyHPSlider.value -= resultingDamage;
            if (EnemyHPSlider.value <= 0) AboutToDie = true;
        }
        else 
        {
            // Possibly make a "Blocked" thing appear?
        }
    }

    public void OnDeath()
    {
        // SOME STUFF IS NEEDED FIRST (EXP)

        MainScript.MoneyScript.Set_Money(OnDeath_Money);

        MainScript.CombatScript.NextEnemyFight();

        /* Instead of Destroy, just push the creature aside and use the death animation */
        Destroy(transform.gameObject);
        /* ==== */
    }
}
