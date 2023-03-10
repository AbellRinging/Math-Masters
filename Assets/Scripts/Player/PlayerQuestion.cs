using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerQuestion : Parent_PlayerScript
{
    private QuestionPanel.Question[] Array_Questions;
    private Dictionary<int, List<QuestionPanel.Question>> Dic_QuestionsByTier;

    private QuestionPanel QuestionScript;

    protected override void Custom_Start()
    {
        Get_AllQuestionsFromJSONs();
        QuestionScript = MainScript.CombatCanvas.transform.Find("Question Panel").GetComponent<QuestionPanel>();
    }
    
    private void Get_AllQuestionsFromJSONs()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Questions");
        Array_Questions = JsonConvert.DeserializeObject<QuestionPanel.Question[]>(textAsset.text);
        
        Dic_QuestionsByTier = new Dictionary<int, List<QuestionPanel.Question>>();

        foreach(QuestionPanel.Question question in Array_Questions)
        {
            if(!Dic_QuestionsByTier.ContainsKey(question.Tier))
            {
                Dic_QuestionsByTier.Add(question.Tier, new List<QuestionPanel.Question>());
            }
            Dic_QuestionsByTier[question.Tier].Add(question);
        }
                /* DEV */ Debug.Log("Number of questions initialized: " + Array_Questions.GetLength(0));
    }

    /// <summary>
    ///     PUBLIC: Generates a question, and returns the answer to said question
    /// </summary>
    public int Generate_NewQuestion(int Tier)
    {
        int answerToQuestion = 0;
        int randomInt = Random.Range(0, Dic_QuestionsByTier[Tier].Count);
        QuestionPanel.Question question = Dic_QuestionsByTier[Tier][randomInt];

        int calculatedResult = Calculate_Result(question);

        string FieldToEmpty = "N/A";
        randomInt = Random.Range(0, 3);
        switch(randomInt)
        {
            case(0):
                FieldToEmpty = "FirstNumber";
                answerToQuestion = question.FirstNumber;
                break;
            case(1):
                FieldToEmpty = "SecondNumber";
                answerToQuestion = question.SecondNumber;
                break;
            case(2):
                FieldToEmpty = "Result";
                answerToQuestion = calculatedResult;
                break;
        }

        QuestionScript.Create_Question(question, FieldToEmpty, calculatedResult);
                /* DEV */ Debug.Log("Answer to Question is " + answerToQuestion);
        return answerToQuestion;
    }

    public bool AttemptAtAnswer(BattleCard.AttackCard attack)
    {
        return QuestionScript.AttemptAtAnswer(int.Parse(attack.ImageName));
    }

    private int Calculate_Result(QuestionPanel.Question question)
    {
        int result = 0;

        switch(question.Operation)
        {
            case("x"):
                result = question.FirstNumber * question.SecondNumber;
                break;
            case("+"):
                result = question.FirstNumber + question.SecondNumber;
                break;
            case("-"):
                result = question.FirstNumber - question.SecondNumber;
                break;
            default:
                Debug.LogError("Operation not recognized in Calculate_Result method. Only accept x / + / -");
                break;
        }
        return result;
    }
}
