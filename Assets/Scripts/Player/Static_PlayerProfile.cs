using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPlayerProfile
{
    public static string PlayerName { get; set; } = "No player name";
    public static int MaxLevelComplete { get; set; } = 0;
    public static int Money { get; set; } = 0;

    public static int MoneyToAdd { get; set; } = 0;
}
