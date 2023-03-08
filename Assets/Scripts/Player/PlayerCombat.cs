using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : Parent_PlayerScript
{
    [Tooltip ("Player will stop further away from the enemy")]
    public float Float_StoppingDistance;    
    [HideInInspector] public List<Enemy> ListOfEnemies;
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
        ListOfEnemies[0].PrepareForCombat();
        MainScript.MovementScript.ForceMoveToLocation(new Vector3(ListOfEnemies[0].transform.position.x, transform.position.y, ListOfEnemies[0].transform.position.z + Float_StoppingDistance));
    }

    /// <summary>
    ///     Where the player interacts with the cards and where the turns pass. Updates every second, assuming there is battle.
    /// </summary>
    // public void Combat_Update()
    // {
    //     /*
    //         Implement Turns here (make use of NewTurn() )
    //     */
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         ListOfEnemies[0].TakeDamage(1);
    //     }
    // }




















    public void BeginFight()
    {
        MainScript.CombatCanvas.SetActive(true);
        Bool_BattleIsHappening = true;
        NewTurn();
    }

    public void NewTurn()
    {
        /*
            MISSING HERE PICKING THE QUESTION AND USING THE ANSWER FOR Generate_NewHand METHOD
        */

        MainScript.DeckScript.Generate_NewHand(1);
    }


    /// <summary>
    ///     Called by a dying enemy INCOMPLETE (EXP GAIN)
    /// </summary>
    public void NextEnemyFight()
    {
        ListOfEnemies.RemoveAt(0);

        if (ListOfEnemies.Count > 0)
        {
            MainScript.CombatScript.Bool_BattleIsHappening = false;
            MainScript.CombatCanvas.SetActive(false);
            ListOfEnemies[0].PrepareForCombat();
            MainScript.MovementScript.ForceMoveToLocation(new Vector3(ListOfEnemies[0].transform.position.x, transform.position.y, ListOfEnemies[0].transform.position.z + Float_StoppingDistance));
        }
        else LevelComplete();
    }

    private void LevelComplete()
    {
        /*
            SAVE PROGFRESS IN DATABASE
        */

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
