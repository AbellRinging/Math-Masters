using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
   public int level;

    private void Awake() 
    {
        if (level > StaticPlayerProfile.MaxLevelComplete + 1)
        {
            transform.GetComponent<Button>().interactable = false;
        }
    }

   public void OpenScene()
   {
       SceneManager.LoadScene("Level" +level.ToString());
   }
}
