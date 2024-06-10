using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset from the player's position
    public float smoothSpeed = 0.125f; // Smooth speed for camera movement

    // Start is called before the first frame update
    void Start()
    {
        // Optional: Initialize the offset to the current difference between the camera and player positions
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Desired position of the camera
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
