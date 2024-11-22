using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the squirrel 
    public float smoothSpeed = 0.125f; // Smoothness factor for following
    public Vector3 offset; // Offset from the target 

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            // Smoothly move the camera towards the desired position using Lerp
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }
    }
}
