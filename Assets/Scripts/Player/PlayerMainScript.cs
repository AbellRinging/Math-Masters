using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMainScript : MonoBehaviour
{
    [Header ("============ Not to be modified ============")]
    #region Player Scripts
        public PlayerMovement MovementScript;
        public PlayerHealth HealthScript;
        public PlayerAnimation AnimationScript;
        public PlayerCamera CameraScript;
        public PlayerCombat CombatScript;
    #endregion
    public Canvas EssentialCanvas;
    public Canvas CombatCanvas;

    [Header ("============  ============")]

    private int int_CurrentScene;

    private void Awake()
    {
        int_CurrentScene = SceneManager.GetActiveScene().buildIndex;

        EssentialCanvas = GameObject.Find("Essential Canvas").GetComponent<Canvas>();

        if(int_CurrentScene != 2) CombatCanvas = GameObject.Find("Battle Canvas").GetComponent<Canvas>();

        #region Script Initializing
            MovementScript = GetComponent<PlayerMovement>();
            HealthScript = GetComponent<PlayerHealth>();
            AnimationScript = GetComponent<PlayerAnimation>();
            CameraScript = GetComponent<PlayerCamera>();
            CombatScript = GetComponent<PlayerCombat>();

            MovementScript.Run_At_Start();
            HealthScript.Run_At_Start();
            AnimationScript.Run_At_Start();
            CameraScript.Run_At_Start();
            if(int_CurrentScene != 2) CombatScript.Run_At_Start();
        #endregion
    }

    private void Update()
    {
        // ## WASD/Mouse-Click Movement and character direction Component. Only usable in Samos Town (Scene 2)
        if(int_CurrentScene == 2) MovementScript.Move();
        // else if (){
            
        // }

        // ## Update Camera Position relative to the player, and allow zoom in and out
        CameraScript.UpdateCamera();
    }
}
