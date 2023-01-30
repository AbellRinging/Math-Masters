using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TextMeshProUGUI TMPro_DisplayName;

    void Awake()
    {
        TMPro_DisplayName.text = "Jogador: " + StaticPlayerProfile.PlayerName;
    }
}
