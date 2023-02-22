using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMoney : Parent_PlayerScript
{
    private TextMeshProUGUI Text_MoneyHUD;

    // Variable that stores money
    public int Money {set; get;}
    protected override void Custom_Start()
    {
        Text_MoneyHUD = MainScript.EssentialCanvas.transform.Find("Coin").GetComponentInChildren<TextMeshProUGUI>();
        Money = MainScript.DB_GetMoney();
        UpdateMoneyHUD(Money);
    }

    public void UpdateMoneyHUD(int amount)
    {
        Text_MoneyHUD.text = "" + Money;
    }
}
