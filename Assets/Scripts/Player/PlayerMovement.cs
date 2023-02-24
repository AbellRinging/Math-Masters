using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : Parent_PlayerScript
{
    #region WASD Movement Variables
        public float movementSpeed = 5f;
        private Vector3 Playerforward;
        private Vector3 right;
        private Rigidbody rb;
    #endregion

    #region Mouse Variables
        private NavMeshAgent NavigationAgent;
        private RaycastHit ClickedLocation;
        private bool isMovingToClickedLocation = false;
        private string groundTag = "Ground";
        public GameObject GO_DisplayClickedLocation;
    #endregion

    protected override void Custom_Start()
    {
        //Rigidbody start
        rb = GetComponent<Rigidbody>();

        //Player WASD Movement start
        Playerforward = Camera.main.transform.forward;
        Playerforward.y = 0;
        Playerforward = Vector3.Normalize(Playerforward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * Playerforward;

        // Mouse Movement start
        NavigationAgent = GetComponent<NavMeshAgent>();
    }

    public void Move()
    {
        // WASD Movement
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) // && !isDashing && !isAttacking)
        {
            // Stop any mouse movement
            isMovingToClickedLocation = false;
            NavigationAgent.velocity = Vector3.zero;
            NavigationAgent.isStopped = true;
            NavigationAgent.ResetPath();
            
            Vector3 rightMovement = right * movementSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 upMovement = Playerforward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
            
            if (heading != Vector3.zero) transform.forward = heading;

            rb.velocity = heading * movementSpeed;

            MainScript.AnimationScript.ToggleAnimation("isWalkingFWD", true);
        }
        // Mouse Click Detection (Only happens if WASD is not being pressed)
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out ClickedLocation, Mathf.Infinity))
            {
                // Clicked on Terrain
                if (ClickedLocation.collider.CompareTag(groundTag))
                {
                    NavigationAgent.SetDestination(ClickedLocation.point);
                    isMovingToClickedLocation = true;

                    Instantiate(GO_DisplayClickedLocation, ClickedLocation.point, Quaternion.identity);
                    MainScript.AnimationScript.ToggleAnimation("isWalkingFWD", true);
                }
                // Clicked on [SOMETHING ELSE] (Call MainScript??) TO BE DEFINED
            }
        }
        // If there is no more player Input, let the character move to the Clicked Location. Checks if the character is still moving
        else if (isMovingToClickedLocation && !NavigationAgent.hasPath)
        {
            isMovingToClickedLocation = false;
        }
        // Stop moving
        else if (!isMovingToClickedLocation)
        {
            rb.velocity = transform.forward * 0;
            MainScript.AnimationScript.ToggleAnimation("isWalkingFWD", false);
        }
    }

    /// <summary>
    ///     Used during Combat, uses NavigationAgent to move the player. Player can not influence this movement
    /// </summary>
    public void ForcedMove()
    {
        if (isMovingToClickedLocation && !NavigationAgent.hasPath)
        {
            isMovingToClickedLocation = false;
        }
        // # Stop moving and allow cards to appear on screen
        else if (!isMovingToClickedLocation)
        {
            MainScript.CombatCanvas.SetActive(true);
            MainScript.AnimationScript.ToggleAnimation("isWalkingFWD", false);
        }
    }

    public void ForceMoveToLocation(Vector3 WhereToGo)
    {
        NavigationAgent.SetDestination(WhereToGo);
        isMovingToClickedLocation = true;

        MainScript.AnimationScript.ToggleAnimation("isWalkingFWD", true);
    }
}

