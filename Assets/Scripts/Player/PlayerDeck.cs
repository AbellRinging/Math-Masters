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
    public GameObject Prefab_Card;

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

    [HideInInspector] public GameObject[] Array_Hand;
    [Tooltip ("How many cards to appear in the hand")]
        public int Int_HandSize = 5;
    [Tooltip("Defines the probability a spell card is drawn. Range -> 1 to 0")]
        public float PercentageToDrawSpell = 0.05f;

    [Tooltip("The amount it takes to draw a card, in seconds")]
        public float DrawingSpeed = 0.2f;
    [Tooltip("The amount it takes to discard a card, in seconds")]
        public float DiscardingSpeed = 0.2f;

    protected override void Custom_Start()
    {
        Array_Hand = new GameObject[Int_HandSize];

        Get_AllCardsFromJSONs();
    }
    
    /// <summary>
    ///     PRIVATE: Reads the JSONs and creates two types of dictionaries: one type containing a name of card -> said card, and the second type is name of card -> sprite of said card
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

        /* DEV */ Debug.Log("Number of attack cards initialized: " + Dic_AttackCard.Count + "\n    Number of spell cards initialized: " + Dic_SpellCard.Count);
    }
    
    #region Generate New Hand
        /// <summary>
        ///     PUBLIC: Generates a new hand of cards, one of which is the answer to the Question. IN CASE OF IMPLEMENTING PROCEDURALY GENERATED LEVELS, MODIFY THIS METHOD??
        /// </summary>
        public void Generate_NewHand(int answerToQuestion)
        {
            Vector3[] cardCoordinates = Generate_CardCoordinates();
            int randomInt = Random.Range(0, cardCoordinates.GetLength(0));

            MainScript.Bool_InterruptableCoroutineIsHappening = true;
            MainScript.InterruptableCoroutine = StartCoroutine(Coroutine_Generate_NewHand(answerToQuestion, cardCoordinates, randomInt));
        }
        private IEnumerator Coroutine_Generate_NewHand(int answerToQuestion, Vector3[] cardCoordinates, int randomInt){
            bool SpellCardDrawn = false;
            for(int i = 0; i < Int_HandSize; ++i)
            {
                if(i != randomInt)
                {
                    float randomfloat = Random.value;

                    // Decide if a spell should be created or not. Only 1 such card can be created per turn
                    if(!SpellCardDrawn && randomfloat <= PercentageToDrawSpell)
                    {
                        int randomInt2 = Random.Range(0, Array_SpellCards.GetLength(0));
                        Array_Hand[i] = Generate_CardPrefab(cardCoordinates[i], Array_SpellCards[randomInt2], Dic_SpellCardSprites[Array_SpellCards[randomInt2].ImageName]);
                        SpellCardDrawn = !SpellCardDrawn;
                    }    
                    else
                    {
                        int randomInt2 = Random.Range(0, Array_AttackCards.GetLength(0));
                        Array_Hand[i] = Generate_CardPrefab(cardCoordinates[i], Array_AttackCards[randomInt2], Dic_AttackCardSprites[Array_AttackCards[randomInt2].ImageName]);
                    }
                }
                else
                {
                    Array_Hand[i] = Generate_CardPrefab(cardCoordinates[i], Dic_AttackCard[answerToQuestion.ToString()], Dic_AttackCardSprites[answerToQuestion.ToString()]);
                            /* DEV */ //Debug.Log("Answer " + answerToQuestion + " was placed in the " + (randomInt + 1) + " index");
                }
                yield return new WaitForSeconds(DrawingSpeed);
            }
            MainScript.Bool_InterruptableCoroutineIsHappening = false;
        }

        /// <summary>
        ///     PRIVATE: Returns an array of coordinates where to instantiate cards
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

    #endregion

    #region Discard Hand
        public bool Bool_Coroutine_Discard_Hand = false;
        /// <summary>
        ///     PUBLIC: Destroys each card in hand. Uses a Coroutine
        /// </summary>
        public void Discard_Hand()
        {
            MainScript.Bool_InterruptableCoroutineIsHappening = true;
            MainScript.InterruptableCoroutine = StartCoroutine(Coroutine_Discard_Hand());
        }
        private IEnumerator Coroutine_Discard_Hand()
        {
            Bool_Coroutine_Discard_Hand = true;
            foreach(GameObject card in Array_Hand)
            {
                if(card != null){
                    yield return new WaitForSeconds(DiscardingSpeed);
                    GameObject.Destroy(card);
                }
            }
            Bool_Coroutine_Discard_Hand = false;
            MainScript.Bool_InterruptableCoroutineIsHappening = false;
        }
    #endregion

    private GameObject Generate_CardPrefab(Vector3 WhereToPut, BattleCard.BaseCard card, Sprite image)
    {
        GameObject newcard = Instantiate(Prefab_Card, MainScript.CombatCanvas.transform);
        newcard.GetComponent<BattleCard>().CreateCard(card, image);
        newcard.GetComponent<BattleCard>().MainScript = MainScript;
        newcard.transform.position = WhereToPut;

        return newcard;
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
