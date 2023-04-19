using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CurrentLevelDisplay : MonoBehaviour
{
    private TextMeshProUGUI textmesh;
    void Awake()
    {
        int currentlevel = SceneManager.GetActiveScene().buildIndex - 2;
        if(currentlevel < 1)
        {
            transform.gameObject.SetActive(false);
            return;
        }
        Debug.Log("Passou");
        textmesh = transform.GetComponent<TextMeshProUGUI>();
        textmesh.text = "Nivel - " + currentlevel;
    }
    void Update()
    {
        Color newcolor = textmesh.color;
        newcolor.a = newcolor.a - 0.40f * Time.deltaTime;
        textmesh.color = newcolor;

        if (newcolor.a <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
