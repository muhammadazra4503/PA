using UnityEngine;
using System.Collections;

public class ShieldGiver : MonoBehaviour
{
    [SerializeField] private float shieldDuration = 5f; // Duration the shield is active
    [SerializeField] private Color shieldColor = Color.blue; // Color when the shield is active
    [SerializeField] private GameObject shieldGameObject; // GameObject that will be activated as a shield
    [SerializeField] private GameObject[] targetGameObjects; // Array of GameObjects for movement
    [SerializeField] private float timeBetweenMoves = 2f; // Time delay between moving to each target

    private int currentTargetIndex = 0; // Index of the current target

    private void OnTriggerEnter(Collider other)
    {
        Shield shield = other.GetComponent<Shield>();
        if (shield != null && shieldGameObject != null)
        {
            // Set duration and color, then activate the shield
            shield.SetShieldDuration(shieldDuration);
            shield.SetShieldColor(shieldColor);
            shield.ActivateShield();

            // Activate the shield GameObject
            shieldGameObject.SetActive(true);

            // Deactivate the shield after the duration
            StartCoroutine(DeactivateShieldAfterDuration());

            // Move to the next target immediately after giving the shield
            MoveToNextTarget();
        }
    }

    private IEnumerator DeactivateShieldAfterDuration()
    {
        yield return new WaitForSeconds(shieldDuration);
        if (shieldGameObject != null)
        {
            shieldGameObject.SetActive(false);
        }
    }

    private void MoveToNextTarget()
    {
        if (targetGameObjects.Length == 0) return;

        // Move to the next target position
        transform.position = targetGameObjects[currentTargetIndex].transform.position;

        // Update the target index, looping back to the start if necessary
        currentTargetIndex = (currentTargetIndex + 1) % targetGameObjects.Length;
    }
}
