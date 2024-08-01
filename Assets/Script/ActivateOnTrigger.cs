using UnityEngine;
using System.Collections; // Tambahkan ini untuk menggunakan IEnumerator

public class ActivateOnTrigger : MonoBehaviour
{
    public GameObject objectToActivate; // GameObject yang akan diaktifkan
    public float delayBeforeActivation = 2f; // Waktu jeda dalam detik

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ganti dengan tag yang sesuai
        {
            StartCoroutine(ActivateObjectWithDelay());
        }
    }

    private IEnumerator ActivateObjectWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeActivation); // Tunggu selama waktu jeda
        objectToActivate.SetActive(true); // Aktifkan GameObject
    }
}
