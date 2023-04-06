using System;
using UnityEngine;
using TMPro;

public class SpellCounter : MonoBehaviour
{
    private TextMeshProUGUI text;

    public void UpdateSpellCounter(int value)
    {
        try
        {
            text.text = "" + value;
        }
        #pragma warning disable 0168
        catch(NullReferenceException ex)
        #pragma warning restore 0168
        {
            text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            UpdateSpellCounter(value);
        }
    }
}
