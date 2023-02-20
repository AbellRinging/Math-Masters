using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{

    public PlayerMovement MovementScript;
    public PlayerHealth HealthScript;
    public PlayerAnimation AnimationScript;
    public PlayerCamera CameraScript;

    private void Awake()
    {
        MovementScript = GetComponent<PlayerMovement>();
        HealthScript = GetComponent<PlayerHealth>();
        AnimationScript = GetComponent<PlayerAnimation>();
        CameraScript = GetComponent<PlayerCamera>();

        MovementScript.Run_At_Start();
        HealthScript.Run_At_Start();
        AnimationScript.Run_At_Start();
        CameraScript.Run_At_Start();
    }

    private void Update()
    {
        // ## WASD/Mouse-Click Movement and character direction Component
        MovementScript.Move();

        // ## Update Camera Position relative to the player, and allow zoom in and out
        CameraScript.UpdateCamera();
    }
}
