using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMoney : Parent_PlayerScript
{
    private TextMeshProUGUI Text_MoneyHUD;
    protected override void Custom_Start()
    {
        Text_MoneyHUD = MainScript.EssentialCanvas.transform.Find("Money").GetComponentInChildren<TextMeshProUGUI>();
        Set_Money(StaticPlayerProfile.Money);
    }

    public void Set_Money (int amount)
    {
        StaticPlayerProfile.MoneyToAdd += amount;
        Text_MoneyHUD.text = "" + StaticPlayerProfile.Money + StaticPlayerProfile.MoneyToAdd;
    }
}
