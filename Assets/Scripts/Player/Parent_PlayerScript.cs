using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent_PlayerScript : MonoBehaviour
{
    protected PlayerMainScript MainScript;

    public void Run_At_Start()
    {
        MainScript = GetComponent<PlayerMainScript>();
        Custom_Start();
    }

    protected virtual void Custom_Start()
    {
        
    }
}
