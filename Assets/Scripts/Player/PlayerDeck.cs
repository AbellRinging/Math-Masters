using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerDeck : Parent_PlayerScript
{

    public AttackCard[] Array_AttackCards;
    public SpellCard[] Array_SpellCards;
    private Dictionary<string, Sprite> Dic_AttackCardSprites;
    private Dictionary<string, Sprite> Dic_SpellCardSprites;

    #region Card Information Classes for the code
        [System.Serializable] public class AttackCard
        {
            public string Name;
            public string Sign;
            public string ImageName;    // Also stores how much damage is dealt since it is a number
        }

        [System.Serializable] public class SpellCard
        {
            public string Name;
            public string ImageName;
            public string SpellType;
        }
    #endregion

    public GameObject Prefab_Card;

    protected override void Custom_Start()
    {
        Get_AllCardsFromJSONs();
        Get_AllCardSprites();
        GameObject newcard = Instantiate(Prefab_Card, MainScript.CombatCanvas.transform);
        newcard.GetComponent<BattleCard>().CreateAttackCard(Array_AttackCards[0], Dic_AttackCardSprites[Array_AttackCards[0].ImageName]);
    }
    
    private void Get_AllCardsFromJSONs()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Attackcards");
        Array_AttackCards = JsonConvert.DeserializeObject<AttackCard[]>(textAsset.text);

        textAsset = Resources.Load<TextAsset>("Spellcards");
        Array_SpellCards = JsonConvert.DeserializeObject<SpellCard[]>(textAsset.text);
    }

    private void Get_AllCardSprites()
    {
        Dic_AttackCardSprites = new Dictionary<string, Sprite>();

        foreach(AttackCard card in Array_AttackCards)
        {
            Dic_AttackCardSprites.Add(card.ImageName, Resources.Load<Sprite>("AttackCardArt/" + card.ImageName));
        }

        // ### Repeat the process above but for Spell cards
        Dic_SpellCardSprites = new Dictionary<string, Sprite>();

        foreach(SpellCard card in Array_SpellCards)
        {
            Dic_SpellCardSprites.Add(card.ImageName, Resources.Load<Sprite>("SpellCardArt/" + card.ImageName));
        }

        Debug.Log("Number of attack cards initialized: " + Dic_AttackCardSprites.Count + "\n    Number of spell cards initialized: " + Dic_SpellCardSprites.Count);
    }
}
