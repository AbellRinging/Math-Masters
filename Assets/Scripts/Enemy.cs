using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header ("Enemy Stats")]
        [Tooltip ("The amount of Health it starts with. This number does not change during runtime")]
        public int MaxHealth;

        [Tooltip ("How difficult will the questions be")]
        public int Tier;
    
    [Header ("Rewards upon Death")]
        [Tooltip ("The amount of EXP rewarded to the player")]
        public int OnDeath_EXP;

        [Tooltip ("The amount of Money rewarded to the player")]
        public int OnDeath_Money;

    private PlayerMainScript MainScript;
    private HeartContainerScript HeartContainer;

    private Animator animator;

    [HideInInspector] public bool AboutToDie = false;

    public void InitializeEnemy(PlayerMainScript PMS)
    {
        animator = GetComponent<Animator>();

        MainScript = PMS;
        HeartContainer = MainScript.CombatCanvas.transform.Find("Enemy").transform.Find("Heart Container").GetComponent<HeartContainerScript>();
    }

    public void PrepareForCombat()
    {
        HeartContainer.SpawnHearts(MaxHealth, false);
    }

    public void TakeDamage()
    {
        AboutToDie = HeartContainer.ReduceHealth();
        TriggerEnemyAnimation("Hit");
        animator.SetBool("Die", AboutToDie);
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

    public void TriggerEnemyAnimation(string AnimationName)
    {
        animator.SetTrigger(AnimationName);
    }

    public void AnimationEnded_ContinueTheGame(int option)
    {
        switch(option)
        {
            case(0):

                break;
            case(1):

                break;
        }
    }
}
