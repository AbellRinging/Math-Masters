using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : Parent_PlayerScript
{
    public Vector3 CameraAngle;

    public float zoomSpeed;
    public float orthographicSizeMin;
    public float orthographicSizeMax;

    protected override void Custom_Start()
    {
        // Nothing happens?
    }

    public void UpdateCamera()
    {
        Camera.main.transform.position = transform.position + CameraAngle;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += zoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize -= zoomSpeed;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
    }
}
