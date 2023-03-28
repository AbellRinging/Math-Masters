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
                            //"Neste turno o inimigo fica impossibilitado de atacar";
                        /* To be implemented*/

                        break;
                    case("ExtraDMG"):
                            //"Durante um turno, tira mais 20% de dano no próximo ataque";
                        /* To be implemented*/

                        break;
                    case("Heal"):
                            //"Cura a personagem em 10 pontos de vida";
                        /* To be implemented*/

                        break;
                    case("DoubleAttack"):
                            //"Durante um turno, ataca duas vezes";
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
            if(MainScript.QuestionScript.AttemptAtAnswer(attack))
            {
                CurrentEnemy.TakeDamage();
            }
            else
            {
                if(MainScript.HealthScript.TakeDamage())
                {
                    // Open Pause Menu
                }
            }
                

            ASpellWasCast = false;
            MainScript.DeckScript.Discard_Hand();

            StartCoroutine(Coroutine_EndTurnWait());
        }
        private IEnumerator Coroutine_EndTurnWait()
        {
            for(;;)
            {
                if (!MainScript.DeckScript.Bool_Coroutine_Discard_Hand)
                {
                    if(CurrentEnemy.AboutToDie) CurrentEnemy.OnDeath();
                    else NewTurn();

                    yield break;
                }
                else yield return new WaitForSeconds(.1f);
            }
        }
    #endregion









    private void SetUpCombat()
    {
        MainScript.CombatScript.Bool_BattleIsHappening = false;
        MainScript.CombatCanvas.SetActive(false);

        CurrentEnemy = ListOfEnemies[0];
        CurrentEnemy.PrepareForCombat();
        MainScript.MovementScript.ForceMoveToLocation(new Vector3(CurrentEnemy.transform.position.x, transform.position.y, CurrentEnemy.transform.position.z + Float_StoppingDistance));
    }

    /// <summary>
    ///     Last Phase of Combat. Called by a dying enemy ### INCOMPLETE (EXP GAIN)
    /// </summary>
    public void NextEnemyFight()
    {
        ListOfEnemies.RemoveAt(0);

        if (ListOfEnemies.Count > 0)
        {
            SetUpCombat();
        }
        else LevelComplete();
    }

    private void LevelComplete()
    {
        /*
            SAVE PROGRESS IN DATABASE
        */

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
