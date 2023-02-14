using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Parent_PlayerScript
{
    public float movementSpeed = 5f;
    private Vector3 Playerforward;
    private Vector3 right;
    private Rigidbody rb;

    protected override void Custom_Start()
    {
        //Rigidbody start
        rb = GetComponent<Rigidbody>();

        //Player WASD Movement start
        Playerforward = Camera.main.transform.forward;
        Playerforward.y = 0;
        Playerforward = Vector3.Normalize(Playerforward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * Playerforward;
    }

    public void Move()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) // && !isDashing && !isAttacking)
        {
            Vector3 rightMovement = right * movementSpeed * Time.deltaTime * Input.GetAxis("Horizontal");

            Vector3 upMovement = Playerforward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
            
            if (heading != Vector3.zero)
            {
                transform.forward = heading;
            }

            rb.velocity = heading * movementSpeed;
            MainScript.AnimationScript.ToggleAnimation("isWalkingFWD", true);
        }
        //Stop movement
        else
        {
            rb.velocity = transform.forward * 0;
            MainScript.AnimationScript.ToggleAnimation("isWalkingFWD", false);
        }
    }
}

