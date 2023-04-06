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
     
    public GameObject EnemyProfile;
    private SpellCounter spellCounter;

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
        SetUpEnemyProfile();

        spellCounter = MainScript.CombatCanvas.transform.Find("Number Of Spells").GetComponent<SpellCounter>();
        spellCounter.UpdateSpellCounter(AmountOfSpellsThePlayerCanUse);
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

        /// <summary>
        ///     To start each turn
        /// </summary>
        public void NewTurn()
        {
            int answerToQuestion = MainScript.QuestionScript.Generate_NewQuestion(CurrentEnemy.Tier);

            MainScript.DeckScript.Generate_NewHand(answerToQuestion);
        }

        #region Spell related Variables
            public int AmountOfSpellsThePlayerCanUse = 5;
            private bool ASpellWasCast = false;     // Used to limit the number of castable spells per turn by one. Also helps with the incrementation (TO BE IMPLEMENTED)
            private bool BlockAttack = false;
            private bool PlayerAttacksTwice = false;
        #endregion
        /// <summary>
        ///     Player picks a spell card. Changes how the battle will happen. Can only happen once per turn
        /// </summary>
        public void SpellCast(BattleCard.SpellCard spell)
        {
            if(AmountOfSpellsThePlayerCanUse <= 0) return;

            if(!ASpellWasCast)
            {
                switch(spell.SpellType){
                    case("Block"):
                            //"Neste turno bloqueias o ataque se errares";
                            BlockAttack = true;
                        break;
                    case("Heal"):
                            //"Cura-te em 1 ponto de vida";
                            MainScript.HealthScript.Heal(1);
                        break;
                    case("DoubleAttack"):
                            //"Durante este turno, causa o dobro do dano se acertares";
                            PlayerAttacksTwice = true;
                        break;
                }
                ASpellWasCast = true;
                AmountOfSpellsThePlayerCanUse--;
                spellCounter.UpdateSpellCounter(AmountOfSpellsThePlayerCanUse);
            }
        }

        /// <summary>
        ///     End of turn after the player picks an attack card. Creature or player takes damage if the picked card is correct or wrong
        /// </summary>
        public void EndTurn(BattleCard.AttackCard attack)
        {

            bool isCorrect = MainScript.QuestionScript.AttemptAtAnswer(attack);

            if(!isCorrect)
            {
                CurrentEnemy.EnemyAttack();

                if(!BlockAttack && MainScript.HealthScript.TakeDamage())
                {
                    MainScript.PauseMenuScript.Pause(true); // Use this line in AnimationEnded type of method, for after the character falls backwards?
                    return;
                }
            }
            else
            {
                CurrentEnemy.TakeDamage(PlayerAttacksTwice);
            }

            ASpellWasCast = false;
            BlockAttack = false;
            PlayerAttacksTwice = false;
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
            SetUpEnemyProfile();
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

    private void SetUpEnemyProfile()
    {
        if(EnemyProfile.transform.childCount != 2)
        {
            Destroy(EnemyProfile.transform.GetChild(2).gameObject);
        }

        GameObject DummyEnemy = Instantiate(CurrentEnemy.gameObject, EnemyProfile.transform);
        DummyEnemy.transform.localPosition = new Vector3(0, 0, -0.3f);
        DummyEnemy.transform.Rotate(0, 40, 0, Space.Self);

        DummyEnemy.layer = 6;
        var children = DummyEnemy.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = 6;
        }

        //DummyEnemy.GetComponent<Enemy>().TriggerEnemyAnimation("BattleReady");
        Destroy(DummyEnemy.GetComponent<Enemy>());
    }
}
