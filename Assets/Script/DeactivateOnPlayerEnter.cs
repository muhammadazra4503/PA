using UnityEngine;
using System.Collections;

public class DeactivateOnPlayerEnter : MonoBehaviour
{
    public GameObject targetObject; // Objek yang akan dinonaktifkan
    public float delay = 2.0f; // Waktu delay sebelum objek dinonaktifkan

    private void OnTriggerEnter(Collider other)
    {
        // Memeriksa apakah objek yang masuk adalah player
        if (other.CompareTag("Player"))
        {
            // Memulai coroutine untuk menonaktifkan objek dengan delay
            StartCoroutine(DeactivateAfterDelay());
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        // Menunggu selama waktu delay
        yield return new WaitForSeconds(delay);

        // Menonaktifkan objek target
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
