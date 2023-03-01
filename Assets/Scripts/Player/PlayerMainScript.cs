using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMainScript : MonoBehaviour
{
    #region Player Scripts
        [HideInInspector] public PlayerMovement MovementScript;
        [HideInInspector] public PlayerHealth HealthScript;
        [HideInInspector] public PlayerAnimation AnimationScript;
        [HideInInspector] public PlayerCamera CameraScript;
        [HideInInspector] public PlayerCombat CombatScript;
        [HideInInspector] public PlayerMoney MoneyScript;
        [HideInInspector] public PlayerDeck DeckScript;
    #endregion
    
    [HideInInspector] public GameObject EssentialCanvas;
    [HideInInspector] public GameObject CombatCanvas;

    private int int_CurrentScene;

    private void Awake()
    {
        int_CurrentScene = SceneManager.GetActiveScene().buildIndex;

        EssentialCanvas = GameObject.Find("Essential Canvas");

        if(int_CurrentScene != 2)
        {
            CombatCanvas = GameObject.Find("Combat Canvas");
            CombatCanvas.SetActive(false);
        } 

        #region Script Initializing
            MovementScript = GetComponent<PlayerMovement>();
            HealthScript = GetComponent<PlayerHealth>();
            AnimationScript = GetComponent<PlayerAnimation>();
            CameraScript = GetComponent<PlayerCamera>();
            CombatScript = GetComponent<PlayerCombat>();
            MoneyScript = GetComponent<PlayerMoney>();
            DeckScript = GetComponent<PlayerDeck>();

            MovementScript.Run_At_Start();
            HealthScript.Run_At_Start();
            AnimationScript.Run_At_Start();
            CameraScript.Run_At_Start();
            if(int_CurrentScene != 2) CombatScript.Run_At_Start();
            MoneyScript.Run_At_Start();
            if(int_CurrentScene != 2) DeckScript.Run_At_Start();
        #endregion
    }

    private void Update()
    {
        // ## Only usable in Samos Town (Scene 2); WASD/Mouse-Click Movement and character direction Component. 
        if(int_CurrentScene == 2) MovementScript.Move();

        // ## Runs for anywhere other than Samos Town
        else
        {
            // ## AI movement to approach enemies
            MovementScript.ForcedMove();

            // ## Combat Interactions
            CombatScript.Combat_Update();
        } 

        // ## Update Camera Position relative to the player, and allow zoom in and out
        CameraScript.UpdateCamera();
    }

    #region Money (Incomplete)
        public int DB_GetMoney()
        {
            return 1;
        }

        public void DB_SetMoney()
        {

        }
    #endregion
}
