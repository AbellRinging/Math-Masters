using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
   public int level;

    // Start is called before the first frame update
    void Start()
    {

    }

   void onTriggerEnter (Collider other)
   {
      SceneManager.LoadScene(2);
   }

   public void OpenScene()
   {
       SceneManager.LoadScene("Level" +level.ToString());
   }
}
