using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeInput : MonoBehaviour
{
    EventSystem system;

    public Selectable firstInput;
    public Button submitButton;

    /*
        THIS SCRIPT IS USED IN THE LOGIN SCREEN
        WHAT IT DOES IS ENABLES THE USE OF TAB TO GO DOWN IN THE LOGIN FIELDS/BUTTONS, AND SHIFT+TAB TO GO UP
        ALSO ENABLES THE USE OF ENTER AS AN ALTERNATIVE TO CLICKING THE LOGIN BUTTON
    */

    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            if (CheckIfSomethingIsSelected()){
                Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                if (previous != null)
                {
                    previous.Select();
                }
            }
            else{
                firstInput.Select();
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (CheckIfSomethingIsSelected()){
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                if (next != null)
                {
                    next.Select();
                }
            }
            else{
                firstInput.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            submitButton.onClick.Invoke();
            Debug.Log("Button Pressed!");
        }
    }

    private bool CheckIfSomethingIsSelected(){
        return system.currentSelectedGameObject != null;
    }
}
