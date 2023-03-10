using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionPanel : MonoBehaviour
{

    /*
        This script is attached to the Question Panel
    */

      #region GameObject Question Panel Visual Components
        public TextMeshProUGUI Text_FirstNumber;
        public TextMeshProUGUI Text_Operation;
        public TextMeshProUGUI Text_SecondNumber;
        public TextMeshProUGUI Text_Result;
    #endregion

    private string EmptyField;
    private Question QuestionInfo;

    private int Result;

    [System.Serializable] public class Question
    {
        public int Tier;
        public int FirstNumber;
        public string Operation;
        public int SecondNumber;
    }

    public void Create_Question(Question question, string FieldToEmpty, int result)
    {
        QuestionInfo = question;
        EmptyField = FieldToEmpty;
        Result = result;

        Text_FirstNumber.text = QuestionInfo.FirstNumber.ToString();
        Text_Operation.text = QuestionInfo.Operation;
        Text_SecondNumber.text = QuestionInfo.SecondNumber.ToString();
        Text_Result.text = Result.ToString();

        switch(EmptyField)
        {
            case("FirstNumber"):
                Text_FirstNumber.text = "?";
                break;
            case("SecondNumber"):
                Text_SecondNumber.text = "?";
                break;
            case("Result"):
                Text_Result.text = "?";
                break;
            default:
                Debug.LogError("Field To Empty not recognized in Create_Question method");
                break;
        }
    }

    public bool AttemptAtAnswer(int answer)
    {
        switch(EmptyField)
        {
            case("FirstNumber"):
                if(answer == QuestionInfo.FirstNumber)
                {
                    Text_FirstNumber.text = QuestionInfo.FirstNumber.ToString();
                    return true;
                }
                break;
            case("SecondNumber"):
                if(answer == QuestionInfo.SecondNumber)
                {
                    Text_SecondNumber.text = QuestionInfo.SecondNumber.ToString();
                    return true;
                }
                break;
            case("Result"):
                if(answer == Result)
                {
                    Text_Result.text = Result.ToString();
                    return true;
                }
                break;
        }
        return false;
    }
}
