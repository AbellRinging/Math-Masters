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

    #region GameObject Card Visual Components
        public TextMeshProUGUI Text_CardName;
        public Image           Image_Card;
        public TextMeshProUGUI Text_Type; // Ataque ou Feitiço
        public TextMeshProUGUI Text_Description;
    #endregion

    #region Card Information Classes for the code
        public class BaseCard 
        { 
            public string Type; 
            public string Name;
            public string ImageName;    // Also stores how much damage is dealt since it is a number, for Attack Cards
        }

        [System.Serializable] public class AttackCard : BaseCard
        {
            public AttackCard()
            {
                Type = "AttackCard";
            }
        }

        [System.Serializable] public class SpellCard : BaseCard
        {
            public string SpellType;

            public SpellCard()
            {
                Type = "SpellCard";
            }
        }
    #endregion

    // ========== Is one or the other, not both
        private AttackCard AttackInfo;
        private SpellCard SpellInfo;
    // ==========

    [HideInInspector] public PlayerMainScript MainScript;

    public Color NormalCard_Color = Color.blue;
    public Color onPointerEnterCard_Color = Color.red;

    #region Interactibility with the cards
        /// <summary>
        ///     When an attack card gets selected, send its card information to CombatScript to end the turn ### DONT FORGET player can use up to one spell per turn before the attack (Will have to call another method in CoombatScript)
        /// </summary>
        public void ClickedCard()
        {
            if(!MainScript.Bool_InterruptableCoroutineIsHappening)
            {
                if(AttackInfo != null)
                {
                    MainScript.CombatScript.EndTurn(AttackInfo);
                    GameObject.Destroy(gameObject);
                }
                else if(SpellInfo != null)
                {
                    MainScript.CombatScript.SpellCast(SpellInfo);
                    //gameObject.SetActive(false);
                    GameObject.Destroy(gameObject);
                }
                else Debug.LogError("Unexpected error in ClickedCard method, card has neither AttackInfo nor SpellInfo");
            }
        }

        public void PointerEnter_Card()
        {
            GetComponent<Image>().color = onPointerEnterCard_Color;
        }

        public void PointerExit_Card()
        {
            GetComponent<Image>().color = NormalCard_Color;
        }
    #endregion

    #region Card Creation
        public void CreateCard(BaseCard card, Sprite image)
        {
            if(card.Type == "AttackCard")
            {
                CreateAttackCard((AttackCard)card, image);
            }
            else
            {
                CreateSpellCard((SpellCard)card, image);
            }
            /* DEV */ //Debug.Log("Created: " + card.Name);
        }

        private void CreateAttackCard(AttackCard card, Sprite image)
        {
            // Save the info
            AttackInfo = card;

            //Change the visuals of the GameObject
            Text_CardName.text = card.Name;
            Image_Card.sprite = image;
            
            Text_Type.text = "Ataque";

            Text_Description.text = "Tentar responder à questão com " + card.ImageName;
        }

        private void CreateSpellCard(SpellCard card, Sprite image)
    {
        // Save the info
        SpellInfo = card;

        //Change the visuals of the GameObject
        Text_CardName.text = card.Name;
        Image_Card.sprite = image;

        Text_Type.text = "Feitiço";

        //Text_Description.fontSize = 15;
        switch(card.SpellType){
            case("Block"):
                Text_Description.text = "Neste turno bloqueias o ataque se errares";
                break;
            case("Heal"):
                Text_Description.text = "Cura-te em 1 ponto de vida";
                break;
            case("DoubleAttack"):
                Text_Description.text = "Durante este turno, causa o dobro do dano se acertares";
                break;
            default:
                Debug.LogError("Type of Spell typed incorrectly. Received: " + card.SpellType);
                break;
        }
    }
    #endregion
}
