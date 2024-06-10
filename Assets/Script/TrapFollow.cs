using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFollowStatic : MonoBehaviour
{
    [SerializeField] private GameObject[] traps;    // Array of traps to follow the player
    [SerializeField] private float chaseSpeed = 2f; // Speed at which the traps will chase the player

    private Transform playerTransform;  // Reference to the player's transform
    private bool isChasing = false;     // Flag to indicate if the traps are chasing the player

    private void Start()
    {
        // Initialize traps array to avoid null reference issues
        if (traps == null || traps.Length == 0)
        {
            Debug.LogError("Traps array is not assigned or empty.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object is tagged as "Player"
        {
            playerTransform = other.transform;  // Set the player's transform
            StartChasing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object is tagged as "Player"
        {
            StopChasing();
        }
    }

    private void Update()
    {
        if (isChasing && playerTransform != null)
        {
            ChasePlayer();
        }
    }

    private void StartChasing()
    {
        isChasing = true;
    }

    private void StopChasing()
    {
        isChasing = false;
    }

    private void ChasePlayer()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i] != null)
            {
                Vector3 direction = (playerTransform.position - traps[i].transform.position).normalized;
                float distanceToPlayer = Vector3.Distance(traps[i].transform.position, playerTransform.position);

                if (distanceToPlayer > 0.1f)  // Stop moving when the trap is close enough to the player
                {
                    traps[i].transform.position += direction * chaseSpeed * Time.deltaTime;
                }
            }
        }
    }
}
