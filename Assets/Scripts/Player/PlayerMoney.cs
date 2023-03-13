using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMoney : Parent_PlayerScript
{
    private TextMeshProUGUI Text_MoneyHUD;
    private int Money;
    protected override void Custom_Start()
    {
        Text_MoneyHUD = MainScript.EssentialCanvas.transform.Find("Money").GetComponentInChildren<TextMeshProUGUI>();
        Money = MainScript.DB_GetMoney();
        Set_Money(1); // ***FETCH AMOUNT OF MONEY FROM DATABASE***
    }

    public void Set_Money (int amount)
    {
        Money += amount;
        Text_MoneyHUD.text = "" + Money;
    }

    public int Get_Money ()
    {
        return Money;
    }
}
