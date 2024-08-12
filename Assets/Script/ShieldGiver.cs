using UnityEngine;
using System.Collections;

public class ShieldGiver : MonoBehaviour
{
    [SerializeField] private float shieldDuration = 5f; // Durasi shield aktif
    [SerializeField] private Color shieldColor = Color.blue; // Warna saat shield aktif
    [SerializeField] private GameObject shieldGameObject; // GameObject yang akan diaktifkan sebagai shield

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
}
