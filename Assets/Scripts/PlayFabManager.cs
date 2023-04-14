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
    public TMP_InputField usernameInput;

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
            Username = usernameInput.text,
            RequireBothUsernameAndEmail = true
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";
        StaticPlayerProfile.PlayerName = usernameInput.text;

        UpdateDisplayName(StaticPlayerProfile.PlayerName);

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

        /*
            THIS NEXT BIT HERE IS HOW TO CALL FOR PROFILE, DON'T FORGET HOW IT WORKS
        */

        PlayFabClientAPI.GetPlayerProfile( new GetPlayerProfileRequest()
        {
            ProfileConstraints = new PlayerProfileViewConstraints(){
                ShowDisplayName = true
            }
        }, result => OnGetPlayerProfileSuccess(result), OnError);

        /*
            ###### 
        */

        GetPrimaryTitleData();
    }

    // Get the values of two primary title data key value pairs
    public void GetPrimaryTitleData()
    {
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, OnGetPrimaryTitleDataSuccess, OnError);
    }

    private void OnGetPrimaryTitleDataSuccess(GetUserDataResult result)
    {
        int value1, value2;
        UserDataRecord userDataRecord1, userDataRecord2;

        if (result.Data.TryGetValue("MaxLevelComplete", out userDataRecord1))
        {
            if (int.TryParse(userDataRecord1.Value, out value1))
            {
                StaticPlayerProfile.MaxLevelComplete = value1;
                Debug.Log("MaxLevelComplete value: " + value1);
            }
            else
            {
                Debug.LogError("Error parsing MaxLevelComplete value: " + userDataRecord1.Value);
                return;
            }
        }

        if (result.Data.TryGetValue("Money", out userDataRecord2))
        {
            if (int.TryParse(userDataRecord2.Value, out value2))
            {
                StaticPlayerProfile.Money = value2;
                Debug.Log("Money value: " + value2);
            }
            else
            {
                Debug.LogError("Error parsing Money value: " + userDataRecord2.Value);
                return;
            }
        }

        StartCoroutine(OnLoginSuccessSleep());
    }

    IEnumerator OnLoginSuccessSleep(){
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    void OnGetPlayerProfileSuccess(GetPlayerProfileResult result){
        StaticPlayerProfile.PlayerName = result.PlayerProfile.DisplayName;
        Debug.Log("User that logged in: " + StaticPlayerProfile.PlayerName);
    }

    void UpdateDisplayName(string Name) {
        PlayFabClientAPI.UpdateUserTitleDisplayName( new UpdateUserTitleDisplayNameRequest {
            DisplayName = Name
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
