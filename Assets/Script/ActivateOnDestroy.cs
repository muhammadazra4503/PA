using UnityEngine;
using System.Collections.Generic;

public class ActivateOnDestroy : MonoBehaviour
{
    public List<GameObject> targetObjects; // List of target objects to check
    public GameObject objectToActivate; // Object to activate after all targetObjects are destroyed

    private bool isActivated = false; // Flag to ensure activation only happens once

    private void Update()
    {
        // Remove all destroyed target objects from the list
        targetObjects.RemoveAll(target => target == null);

        // If all target objects are destroyed and the object hasn't been activated yet, activate it
        if (targetObjects.Count == 0 && !isActivated)
        {
            objectToActivate.SetActive(true);
            isActivated = true; // Set the flag to true to prevent further activations
            Debug.Log("All target objects are destroyed. Another object is activated.");
        }
    }
}
