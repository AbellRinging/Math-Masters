using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleCard : MonoBehaviour
{
    public class Card
    {
        public string Name;
        public string Sign;
        public Sprite Image;
        public string Type;
        public int Damage;          // For Attacks only
        public string SpellType;    // For Spells only

        // For creating Attack Cards
        public Card(string cardName, string sign, Sprite attackImage, int attackDamage)
        {
            Name = cardName;
            Sign = sign;
            Image = attackImage;
            Type = "Ataque";
            Damage = attackDamage;
        }

        // For creating Spell Cards
        public Card(string cardName, Sprite spellImage, string typeOfSpell)
        {
            Name = cardName;
            Sign = " ";
            Image = spellImage;
            Type = "Feitiço";
            SpellType = typeOfSpell;
        }
    }

    public Card SavedCard;

    #region GameObject Card Private Properties
        private TextMeshProUGUI Text_CardName;
        private TextMeshProUGUI Text_Sign;
        private Image           Image_Card;
        private TextMeshProUGUI Text_Type; // Ataque ou Feitiço
        private TextMeshProUGUI Text_Description;
    #endregion

    public void CreateCard(Card cardInfo)
    {
        SavedCard = cardInfo;
        
        if (SavedCard.Type == "Ataque")
        {
            CreateAttackCard();
        }
        else if (SavedCard.Type == "Feitiço")
        {
            CreateSpellCard();
        }
        else Debug.LogError("Type of Card typed incorrectly. Received: " + SavedCard.Type);
    }
    public void CreateAttackCard()
    {
        Text_CardName.text = SavedCard.Name;
        Text_Sign.text = SavedCard.Sign;
        Image_Card.sprite = SavedCard.Image;
        
        Text_Type.text = "Ataque";

        Text_Description.text = "Dá " + SavedCard.Damage + " de dano ao inimigo";
    }

    public void CreateSpellCard()
    {
        Text_CardName.text = SavedCard.Name;
        Text_Sign.text = " ";
        Image_Card.sprite = SavedCard.Image;

        Text_Type.text = "Feitiço";

        switch(SavedCard.SpellType){
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
                Debug.LogError("Type of Spell typed incorrectly. Received: " + SavedCard.SpellType);
                break;
        }
    }
}
