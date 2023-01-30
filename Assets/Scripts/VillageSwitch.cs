using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageSwitch : MonoBehaviour
{
   void onTriggerEnter (Collider other)
   {
      if(other.CompareTag("Player"))
    {
      SceneManager.LoadScene(2);
    }
   }

}
