using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public void RemoveTutorial ()
    {
        Destroy(transform.gameObject);
    }
}
