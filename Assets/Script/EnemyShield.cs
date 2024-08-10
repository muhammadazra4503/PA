using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth; // Referensi ke script EnemyHealth
    [SerializeField] private GameObject shieldObject; // GameObject yang menjadi shield
    [SerializeField] private GameObject objectToDestroy; // GameObject yang harus dihancurkan untuk mematikan shield

    private bool isShieldActive = false;

    private void Start()
    {
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth is not assigned. Please assign it in the Inspector.");
        }

        if (shieldObject == null)
        {
            Debug.LogError("ShieldObject is not assigned. Please assign it in the Inspector.");
        }

        if (objectToDestroy == null)
        {
            Debug.LogError("ObjectToDestroy is not assigned. Please assign it in the Inspector.");
        }

        shieldObject.SetActive(false); // Pastikan shield tidak aktif pada awalnya
    }

    private void Update()
    {
        if (enemyHealth.CurrentHealth <= 1 && !isShieldActive)
        {
            ActivateShield();
        }

        if (isShieldActive && objectToDestroy == null)
        {
            DeactivateShield();
        }
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        shieldObject.SetActive(true);
        Debug.Log("Shield activated.");
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        shieldObject.SetActive(false);
        Debug.Log("Shield deactivated.");
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}
