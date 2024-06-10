using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomReset : MonoBehaviour
{
    [SerializeField] private GameObject[] traps;    // Array of traps to reset
    private Vector3[] initialPositions; // Array to store the initial positions of the traps

    private void Start()
    {
        // Initialize traps array to avoid null reference issues
        if (traps == null || traps.Length == 0)
        {
            Debug.LogError("Traps array is not assigned or empty.");
            return;
        }

        // Store initial positions of the traps
        initialPositions = new Vector3[traps.Length];
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i] != null)
            {
                initialPositions[i] = traps[i].transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object is tagged as "Player"
        {
            Debug.Log("Player entered the room");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object is tagged as "Player"
        {
            Debug.Log("Player exited the room");
            ResetTraps(); // Reset traps to their initial positions
        }
    }

    private void ResetTraps()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i] != null)
            {
                traps[i].transform.position = initialPositions[i];
            }
        }
    }
}
