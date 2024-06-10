using UnityEngine;
using UnityEngine.Events;

public class TriggerHandler : MonoBehaviour
{
    // Define UnityEvents that can be assigned in the Inspector
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    // This method is called when another collider enters the trigger collider attached to the GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check the tag of the other collider
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger zone.");
            // Invoke the event for entering the trigger if it has listeners
            if (OnPlayerEnter != null)
            {
                OnPlayerEnter.Invoke();
            }
            else
            {
                Debug.LogWarning("OnPlayerEnter event has no listeners assigned.");
            }
        }
    }

    // This method is called when another collider exits the trigger collider attached to the GameObject
    private void OnTriggerExit(Collider other)
    {
        // Check the tag of the other collider
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has exited the trigger zone.");
            // Invoke the event for exiting the trigger if it has listeners
            if (OnPlayerExit != null)
            {
                OnPlayerExit.Invoke();
            }
            else
            {
                Debug.LogWarning("OnPlayerExit event has no listeners assigned.");
            }
        }
    }
}
