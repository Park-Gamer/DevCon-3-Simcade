using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the squirrel 
    public float smoothTime = 0.3f; // Damping time for smoothing
    public Vector3 offset; // Offset from the target 

    private Vector3 currentVelocity = Vector3.zero; 

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position using SmoothDamp
            // SmoothDamp transitions and slows down the camera as it approaches the target position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

            transform.LookAt(target);
        }
    }
}
