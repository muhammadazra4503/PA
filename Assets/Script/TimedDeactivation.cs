using UnityEngine;
using System.Collections;

public class TimedDeactivation : MonoBehaviour
{
    [Header("Deactivation Settings")]
    [SerializeField] private float deactivationTime = 5.0f; // Time after which the object will be deactivated

    private void OnEnable()
    {
        // Start the deactivation coroutine when the object is enabled
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(deactivationTime);
        
        // Deactivate the GameObject
        gameObject.SetActive(false);
    }
}
