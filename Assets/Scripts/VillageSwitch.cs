using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageSwitch : MonoBehaviour
{
   void onTriggerEnter (Collider other)
   {
    print("Trigger Entered");
    
    if(other.tag == "Player")
    {
        print("Switch Scene to 2");
        SceneManager.LoadScene(2);
    }
   }

}
