using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleCard : MonoBehaviour
{
    /*
        This script is attached to the cards
    */

    #region Card Information Classes for the code
        public class BaseCard {/* No purpose other than unifying the two types for the Hand array in PlayerDeck script*/}

        [System.Serializable] public class AttackCard : BaseCard
        {
            public string Type = "AttackCard";

            public string Name;
            public string Sign;
            public string ImageName;    // Also stores how much damage is dealt since it is a number
        }

        [System.Serializable] public class SpellCard : BaseCard
        {
            public string Type = "SpellCard";

            public string Name;
            public string ImageName;
            public string SpellType;
        }
    #endregion

    #region GameObject Card Visual Components
        public TextMeshProUGUI Text_CardName;
        public TextMeshProUGUI Text_Sign;
        public Image           Image_Card;
        public TextMeshProUGUI Text_Type; // Ataque ou Feitiço
        public TextMeshProUGUI Text_Description;
    #endregion

    // ========== Is one or the other, not both
        private AttackCard AttackInfo;
        private SpellCard SpellInfo;
    // ==========

    public void CreateAttackCard(AttackCard card, Sprite image)
    {
        // Save the info
        AttackInfo = card;

        //Change the visuals of the GameObject
        Text_CardName.text = card.Name;
        Text_Sign.text = card.Sign;
        Image_Card.sprite = image;
        
        Text_Type.text = "Ataque";

        Text_Description.text = "Dá " + card.ImageName + " de dano ao inimigo";
    }

    public void CreateSpellCard(SpellCard card, Sprite image)
    {
        // Save the info
        SpellInfo = card;

        //Change the visuals of the GameObject
        Text_CardName.text = card.Name;
        Text_Sign.text = " ";
        Image_Card.sprite = image;

        Text_Type.text = "Feitiço";

        switch(card.SpellType){
            case("Suspend"):
                Text_Description.text = "Neste turno o inimigo fica impossibilitado de atacar";
                break;
            case("ExtraDMG"):
                Text_Description.text = "Durante um turno, tira mais 20% de dano no próximo ataque";
                break;
            case("Heal"):
                Text_Description.text = "Cura a personagem em 10 pontos de vida";
                break;
            case("DoubleAttack"):
                Text_Description.text = "Durante um turno, ataca duas vezes";
                break;
            default:
                Debug.LogError("Type of Spell typed incorrectly. Received: " + card.SpellType);
                break;
        }
    }
}
