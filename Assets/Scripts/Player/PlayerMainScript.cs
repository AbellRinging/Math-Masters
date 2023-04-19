using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

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
        [HideInInspector] public PlayerQuestion QuestionScript;
    #endregion
    
    [HideInInspector] public GameObject EssentialCanvas;
    [HideInInspector] public PauseMenu PauseMenuScript;
    [HideInInspector] public GameObject CombatCanvas;

    public int int_CurrentScene;

    #region Coroutine Storage
        /// <summary>
        ///     PUBLIC: Where the currently active Coroutines are stored. Check using 'Bool_InterruptableCoroutineIsHappening'
        /// </summary>
        [HideInInspector] public Coroutine InterruptableCoroutine;
        [HideInInspector] public bool Bool_InterruptableCoroutineIsHappening;
    #endregion

    private void Awake()
    {
        int_CurrentScene = SceneManager.GetActiveScene().buildIndex;

        EssentialCanvas = GameObject.Find("Essential Canvas");
        PauseMenuScript = EssentialCanvas.GetComponent<PauseMenu>();
        PauseMenuScript.SpecifyPauseMenu(this, int_CurrentScene);

        if(int_CurrentScene != 1) CombatCanvas = GameObject.Find("Combat Canvas"); 

        #region Script Initializing
            MovementScript = GetComponent<PlayerMovement>();
            HealthScript = GetComponent<PlayerHealth>();
            AnimationScript = GetComponent<PlayerAnimation>();
            CameraScript = GetComponent<PlayerCamera>();
            CombatScript = GetComponent<PlayerCombat>();
            MoneyScript = GetComponent<PlayerMoney>();
            DeckScript = GetComponent<PlayerDeck>();
            QuestionScript = GetComponent<PlayerQuestion>();

            MovementScript.Run_At_Start();
            HealthScript.Run_At_Start();
            AnimationScript.Run_At_Start();
            CameraScript.Run_At_Start();
            if(int_CurrentScene != 1) CombatScript.Run_At_Start();
            MoneyScript.Run_At_Start();
            if(int_CurrentScene != 1) DeckScript.Run_At_Start();
            if(int_CurrentScene != 1) QuestionScript.Run_At_Start();
        #endregion

        UpdateNameAndXPInHUD();
        StaticPlayerProfile.MoneyToAdd = 0;
    }

    private void Update()
    {
        if(!PauseMenuScript.GameIsPaused)
        {
            // ## Only usable in Samos Town (Scene 2); WASD/Mouse-Click Movement and character direction Component. 
            if(int_CurrentScene == 1) MovementScript.Move();

            // ## Runs for anywhere other than Samos Town
            else
            {
                // ## AI movement to approach enemies
                MovementScript.ForcedMove();
            } 

            // ## Update Camera Position relative to the player, and allow zoom in and out
            CameraScript.UpdateCamera();
        }
    }

    #region Database
        // Set the values of two primary title data key value pairs
        public void SetPrimaryTitleData()
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {"MaxLevelComplete", "" + StaticPlayerProfile.MaxLevelComplete},
                    {"Money", "" + StaticPlayerProfile.Money}
                }
            };
            PlayFabClientAPI.UpdateUserData(request, OnSetPrimaryTitleDataSuccess, OnError);
        }
        public void SetPrimaryTitleData(string key, string value)
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { key, value }
                }
            };

            PlayFabClientAPI.UpdateUserData(request, OnSetPrimaryTitleDataSuccess, OnError);
        }

        private void OnSetPrimaryTitleDataSuccess(UpdateUserDataResult result)
        {
            PauseMenuScript.AllowPlayerToContinueInEndOfLevelMenu();
            Debug.Log("Primary title data updated successfully.");
        }

        private void OnError(PlayFabError error)
        {
            PauseMenuScript.AllowPlayerToRetry();
            Debug.LogError("PlayFab error: " + error.ErrorMessage);
        }
    #endregion

    #region UI related
        private Transform NameAndXP;
        private void UpdateNameAndXPInHUD()
        {
            NameAndXP = EssentialCanvas.transform.GetChild(1).transform.GetChild(0);
            NameAndXP.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = StaticPlayerProfile.PlayerName;
            NameAndXP.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + StaticPlayerProfile.MaxLevelComplete;
        }
    #endregion
}
