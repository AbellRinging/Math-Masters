using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class PlayerDeck : Parent_PlayerScript
{
    /*
        ## Used by the player prefab. Stores the cards' info in runtime, and is where the hand is located.
        ADD A BUNCH OF TIMERS TO GIVE ROOM FOR ANIMATIONS
    */
    [HideInInspector] public GameObject Prefab_Card;

    #region Card Database to get information when generating cards in the UI
        /* For grabbing a random card */
        private BattleCard.AttackCard[] Array_AttackCards;
        private BattleCard.SpellCard[] Array_SpellCards;
        /* ==== */

        /* For searching specific cards */
            private Dictionary<string, BattleCard.AttackCard> Dic_AttackCard;
            private Dictionary<string, BattleCard.SpellCard> Dic_SpellCard;
        /* ==== */

        /* Regardless of card search, sprites are specific to each card */
        private Dictionary<string, Sprite> Dic_AttackCardSprites;
        private Dictionary<string, Sprite> Dic_SpellCardSprites;
        /* ==== */
    #endregion

    [HideInInspector] public BattleCard.BaseCard[] Array_Hand;
    [Tooltip ("How many cards to appear in the hand")]
        public int Int_HandSize = 5;

    protected override void Custom_Start()
    {
        Array_Hand = new BattleCard.BaseCard[Int_HandSize];

        Get_AllCardsFromJSONs();

        Generate_NewHand(1);
    }
    
    /// <summary>
    ///     Reads the JSONs and creates two types of dictionaries: one type containing a name of card -> said card, and the second type is name of card -> sprite of said card
    /// </summary>
    private void Get_AllCardsFromJSONs()
    {
        /*
            ### Attack Cards
        */

        TextAsset textAsset = Resources.Load<TextAsset>("Attackcards");
        Array_AttackCards = JsonConvert.DeserializeObject<BattleCard.AttackCard[]>(textAsset.text);
        
        Dic_AttackCardSprites = new Dictionary<string, Sprite>();
        Dic_AttackCard = new Dictionary<string, BattleCard.AttackCard>();

        foreach(BattleCard.AttackCard card in Array_AttackCards)
        {
            Dic_AttackCardSprites.Add(card.ImageName, Resources.Load<Sprite>("AttackCardArt/" + card.ImageName));
            Dic_AttackCard.Add(card.ImageName, card);
        }

        /*
            ### Spell Cards
        */

        textAsset = Resources.Load<TextAsset>("Spellcards");
        Array_SpellCards = JsonConvert.DeserializeObject<BattleCard.SpellCard[]>(textAsset.text);

        Dic_SpellCardSprites = new Dictionary<string, Sprite>();
        Dic_SpellCard = new Dictionary<string, BattleCard.SpellCard>();

        foreach(BattleCard.SpellCard card in Array_SpellCards)
        {
            Dic_SpellCardSprites.Add(card.ImageName, Resources.Load<Sprite>("SpellCardArt/" + card.ImageName));
            Dic_SpellCard.Add(card.ImageName, card);
        }

        Debug.Log("Number of attack cards initialized: " + Dic_AttackCard.Count + "\n    Number of spell cards initialized: " + Dic_SpellCard.Count);
    }

    
    /// <summary>
    ///     Returns an array of coordinates where to instantiate cards
    /// </summary>
    private Vector3[] Generate_CardCoordinates()
    {
        Vector3[] cardCoordinates = new Vector3[Int_HandSize];
        
        Vector2 canvasSize = MainScript.CombatCanvas.GetComponent<CanvasScaler>().referenceResolution;
        float distanceBetweenCards = canvasSize.x / (Int_HandSize + 1);
        
        for(int i = 0; i < Int_HandSize; ++i)
        {
            cardCoordinates[i] = RT_DeckMat.position + new Vector3(distanceBetweenCards * (i + 1) - canvasSize.x/2, 0, 0);
        }

        return cardCoordinates;
    }

    /// <summary>
    ///     Generates a new hand of cards, one of which is the answer to the Question. IN CASE OF IMPLEMENTING PROCEDURALY GENERATED LEVELS, MODIFY THIS METHOD??
    /// </summary>
    public void Generate_NewHand(int answerToQuestion)
    {
        Vector3[] cardCoordinates = Generate_CardCoordinates();

        GameObject newcard;
        for(int i = 0; i < Int_HandSize; ++i)
        {
            // INCOMPLETE, NEEDS RNG TO PICK CARDS AND DISTRIBUTE THEM RANDOMLY
            newcard = Instantiate(Prefab_Card, MainScript.CombatCanvas.transform);
            newcard.GetComponent<BattleCard>().CreateAttackCard(Dic_AttackCard[answerToQuestion.ToString()], Dic_AttackCardSprites[answerToQuestion.ToString()]);
            newcard.transform.position = cardCoordinates[i];
        }
    }






    /*
        FOR DEBUGGING PURPOSES ONLY
    */
    #region Draw Gizmos
        [Header("Draw Gizmos -> For Debugging purposes")]
        public bool Toggle = false;
        public RectTransform RT_DeckMat;
        public CanvasScaler cs;
        public Vector3 CardDimensions;

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.red;
            if(Toggle)
            {
                // ================ This works
                Vector2 canvasSize = cs.referenceResolution;
                Gizmos.DrawSphere(new Vector3(canvasSize.x, RT_DeckMat.position.y, 0), 5f);
                // ================
                
                float distanceBetweenCards = canvasSize.x / (Int_HandSize + 1);

                // Gizmos.DrawSphere(GO_DeckMat.transform.position, 10f);
                for(int i = 1; i < Int_HandSize + 1; ++i)
                {
                    Gizmos.DrawWireCube(RT_DeckMat.position + new Vector3(distanceBetweenCards * i - canvasSize.x/2, 0, 0) , CardDimensions);
                }
            }
        }
    #endregion
}
