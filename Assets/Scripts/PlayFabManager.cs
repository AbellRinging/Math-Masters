using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    /*
        That Login() method in Start() was to login with computer ID and not with email + password;
    */

    // private void Start()
    // {
    //     Login();
    // } 

    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";
        StaticPlayerProfile.PlayerFabId = result.PlayFabId;
        UpdateDisplayName(StaticPlayerProfile.PlayerFabId);
        StartCoroutine(OnLoginSuccessSleep());
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in!";
        StaticPlayerProfile.PlayerFabId = result.PlayFabId;

        /*
            THIS NEXT BIT HERE IS HOW TO CALL FOR PROFILE, DON'T FORGET HOW IT WORKS
        */

        PlayFabClientAPI.GetPlayerProfile( new GetPlayerProfileRequest(){
            PlayFabId = StaticPlayerProfile.PlayerFabId,
            ProfileConstraints = new PlayerProfileViewConstraints(){
                ShowDisplayName = true
            }
        }, result => OnGetPlayerProfileSuccess(result), OnError);

        /*
            ###### 
        */

        StartCoroutine(OnLoginSuccessSleep());
    }

    IEnumerator OnLoginSuccessSleep(){
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    void OnGetPlayerProfileSuccess(GetPlayerProfileResult result){
        StaticPlayerProfile.PlayerName = result.PlayerProfile.DisplayName;
    }


    void UpdateDisplayName(string PlayFabId) {
        PlayFabClientAPI.UpdateUserTitleDisplayName( new UpdateUserTitleDisplayNameRequest {
            DisplayName = PlayFabId
        }, result => {
            Debug.Log("The player's display name is now: " + result.DisplayName);
        }, OnError);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "30F71"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Changed reset mail sent";
    }

    // void Login()
    // {
    //     var request = new LoginWithCustomIDRequest
    //     {
    //         CustomId = SystemInfo.deviceUniqueIdentifier,
    //         CreateAccount = true
    //     };
    //     PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    // }

    // void OnSuccess(LoginResult result)
    // {
    //     messageText.text = "Successful login/account create!";
    // }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
}
