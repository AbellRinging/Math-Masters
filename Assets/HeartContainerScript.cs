using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainerScript : MonoBehaviour
{
    public GameObject HeartPrefab;
    public Sprite[] HeartSprite; // 1 is Full Heart, 0 is Empty Heart

    private int Amount;
    private RectTransform Area;
    private GameObject[] Hearts;
    private int CurrentHealth;

    public void SpawnHearts(int amount, bool LeftToRight)
    {   
        if(Hearts != null)
        {
            foreach(GameObject heart in Hearts)
            {
                GameObject.Destroy(heart);
            }
        }

        Amount = amount;
        CurrentHealth = Amount;
        Hearts = new GameObject[Amount];
        Area = transform.GetComponent<RectTransform>();

        Generate_HeartPrefabs(LeftToRight);
    }

    private void Generate_HeartPrefabs(bool LeftToRight)
    {
        Vector3[] Coordinates = Generate_HeartCoordinates(LeftToRight);

        for(int i = 0; i < Amount; i++)
        {
            Hearts[i] = Instantiate(HeartPrefab, Area.transform);
            Hearts[i].transform.position = Coordinates[i];
            Hearts[i].GetComponent<Image>().sprite = HeartSprite[1];
        }
    }
    private Vector3[] Generate_HeartCoordinates(bool LeftToRight)
    {
        Vector3[] heartCoordinates = new Vector3[Amount];
        
        Vector2 AreaSize = new Vector2(Area.rect.width, Area.rect.height);
        float distanceBetweenHearts = AreaSize.x / (Amount + 1);
        
        for(int i = 0; i < Amount; ++i)
        {
            if(LeftToRight)
            {
                heartCoordinates[i] = Area.position + new Vector3(distanceBetweenHearts * (i + 1) - AreaSize.x/2, 0, 0);
            }
            else
            {
                heartCoordinates[i] = Area.position - new Vector3(distanceBetweenHearts * (i + 1) - AreaSize.x/2, 0, 0);
            }
        }

        return heartCoordinates;
    }


    public bool ReduceHealth()
    {
        Hearts[CurrentHealth - 1].GetComponent<Image>().sprite = HeartSprite[0];
        CurrentHealth--;

        if(CurrentHealth == 0) // Die
        {
            return true;
        }

        return false;
    }

    public void RegenerateHealth()
    {
        if(CurrentHealth == Amount)
        {
            Hearts[CurrentHealth - 1].GetComponent<Image>().sprite = HeartSprite[1];
            CurrentHealth++;
        }
    }
}
