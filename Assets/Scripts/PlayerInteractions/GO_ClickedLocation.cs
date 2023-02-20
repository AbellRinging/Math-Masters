using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GO_ClickedLocation : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(Ephemerate());
    }
    
    IEnumerator Ephemerate(){
        yield return new WaitForSeconds(0.3f);
        Destroy(transform.gameObject);
    }
}
