using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : Parent_PlayerScript
{
    [Tooltip ("Player will stop further away from the enemy")]
        public float Float_StoppingDistance;    
    [HideInInspector] public List<Enemy> ListOfEnemies;
    [HideInInspector] public Enemy CurrentEnemy;
    [HideInInspector] public int EnemyIndexInList = 0;
    [HideInInspector] public bool Bool_BattleIsHappening = false;
    



    protected override void Custom_Start()
    {
        ListOfEnemies = new List<Enemy>();
        foreach (Enemy enemy in GameObject.Find("Enemies").GetComponentsInChildren<Enemy>())
        {
                    /* DEV */ //Debug.Log(enemy.gameObject.name);
            ListOfEnemies.Add(enemy);
            enemy.InitializeEnemy(MainScript);
        }

        SetUpCombat();
    }

    /// <summary>
    ///     First Phase of Combat
    /// </summary>
    public void BeginFight()
    {
        CurrentEnemy.GetComponent<Enemy>().TriggerEnemyAnimation("BattleReady");
        MainScript.CombatCanvas.SetActive(true);
        Bool_BattleIsHappening = true;
        NewTurn();
    }



    #region Turn phases
        private bool ASpellWasCast = false;     // Used to limit the number of castable spells per turn by one.

        /// <summary>
        ///     To start each turn
        /// </summary>
        public void NewTurn()
        {
            int answerToQuestion = MainScript.QuestionScript.Generate_NewQuestion(CurrentEnemy.Tier);

            MainScript.DeckScript.Generate_NewHand(answerToQuestion);
        }

        
        /// <summary>
        ///     Player picks a spell card. Changes how the battle will happen. Can only happen once per turn
        /// </summary>
        public void SpellCast(BattleCard.SpellCard spell)
        {
            if(!ASpellWasCast)
            {
                switch(spell.SpellType){
                    case("Suspend"):
                            //"Neste turno o inimigo fica impossibilitado de atacar se errares";
                        /* To be implemented*/

                        break;
                    case("Heal"):
                            //"Cura-te em 1 ponto de vida";
                        /* To be implemented*/

                        break;
                    case("DoubleAttack"):
                            //"Durante este turno, causa o dobro do dano se acertares";
                        /* To be implemented*/

                        break;
                }

                ASpellWasCast = true;
            }
        }

        /// <summary>
        ///     End of turn after the player picks an attack card. Creature or player takes damage if the picked card is correct or wrong
        /// </summary>
        public void EndTurn(BattleCard.AttackCard attack)
        {

            if(!MainScript.QuestionScript.AttemptAtAnswer(attack))
            {
                CurrentEnemy.EnemyAttack();

                if(MainScript.HealthScript.TakeDamage())
                {
                    MainScript.PauseMenuScript.Pause(true); // Use this line in AnimationEnded type of method, for after the character falls backwards?
                    return;
                }
            }
            else
            {
                CurrentEnemy.TakeDamage();
            }
            
            ASpellWasCast = false;
            MainScript.DeckScript.Discard_Hand();
        }
    #endregion


    private void SetUpCombat()
    {
        MainScript.CombatScript.Bool_BattleIsHappening = false;
        MainScript.CombatCanvas.SetActive(false);

        CurrentEnemy = ListOfEnemies[EnemyIndexInList];
        CurrentEnemy.PrepareForCombat();
        MainScript.MovementScript.ForceMoveToLocation(new Vector3(CurrentEnemy.transform.position.x, transform.position.y, CurrentEnemy.transform.position.z + Float_StoppingDistance));
    }

    /// <summary>
    ///     Last Phase of Combat. Called by a dying enemy ### INCOMPLETE (EXP GAIN)
    /// </summary>
    public void NextEnemyFight()
    {
        EnemyIndexInList++;

        if (EnemyIndexInList < ListOfEnemies.Count)
        {
            SetUpCombat();
        }
        else LevelComplete();
    }

    private void LevelComplete()
    {
        MainScript.PauseMenuScript.EndOfLevelMenu();

        /*
            SAVE PROGRESS IN DATABASE
        */

        StartCoroutine(ManuallyInsertedDelay());
    }
    private IEnumerator ManuallyInsertedDelay() //  Put the content in the method above?
    {
        yield return new WaitForSeconds(1);
        MainScript.PauseMenuScript.AllowPlayerToContinueInEndOfLevelMenu();
    }
}
