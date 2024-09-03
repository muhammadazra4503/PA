using UnityEngine;
using System.Collections;

public class ObjectActivator : MonoBehaviour
{
    public GameObject[] targetObjects; // Array of objects to be activated and deactivated
    public float[] activeDurations; // Array of active durations for each object
    public float[] inactiveDurations; // Array of inactive durations for each object

    void Start()
    {
        // Start a coroutine for each object
        for (int i = 0; i < targetObjects.Length; i++)
        {
            StartCoroutine(ActivateAndDeactivate(targetObjects[i], activeDurations[i], inactiveDurations[i]));
        }
    }

    IEnumerator ActivateAndDeactivate(GameObject obj, float activeDuration, float inactiveDuration)
    {
        while (true) // Infinite loop
        {
            // Activate the object
            obj.SetActive(true);

            // Wait for the active duration
            yield return new WaitForSeconds(activeDuration);

            // Deactivate the object
            obj.SetActive(false);

            // Wait for the inactive duration
            yield return new WaitForSeconds(inactiveDuration);
        }
    }
}
