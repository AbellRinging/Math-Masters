using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : Parent_PlayerScript
{
    [Tooltip ("Player will stop further away from the enemy")]
    public float Float_StoppingDistance;    
    [HideInInspector] public List<Enemy> ListOfEnemies;

    protected override void Custom_Start()
    {
        ListOfEnemies = new List<Enemy>();
        foreach (Enemy enemy in GameObject.Find("Enemies").GetComponentsInChildren<Enemy>())
        {
            Debug.Log(enemy.gameObject.name);
            ListOfEnemies.Add(enemy);
            enemy.InitializeEnemy(MainScript);
        }
        ListOfEnemies[0].PrepareForCombat();
        MainScript.MovementScript.ForceMoveToLocation(new Vector3(ListOfEnemies[0].transform.position.x, transform.position.y, ListOfEnemies[0].transform.position.z + Float_StoppingDistance));
    }

    /// <summary>
    ///     Updates every second, assuming there is battle ***VERY INCOMPLETE*** Missing card interactions here
    /// </summary>
    public void Combat_Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ListOfEnemies[0].TakeDamage(1);
        }
    }


    /// <summary>
    ///     Called by a dying enemy INCOMPLETE
    /// </summary>
    public void NextEnemyFight()
    {
        ListOfEnemies.RemoveAt(0);

        if (ListOfEnemies.Count > 0)
        {
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
