using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float shieldDuration = 5f; // Default durasi shield aktif
    private Color shieldColor = Color.blue; // Default warna shield
    private Color originalColor;

    private Health healthScript;
    private SpriteRenderer spriteRenderer;
    private bool shieldActive = false;

    private void Awake()
    {
        healthScript = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void SetShieldDuration(float duration)
    {
        shieldDuration = duration;
    }

    public void SetShieldColor(Color color)
    {
        shieldColor = color;
    }

    public void ActivateShield()
    {
        if (shieldActive) return;
        shieldActive = true;

        StartCoroutine(ShieldCoroutine());
    }

    private IEnumerator ShieldCoroutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = shieldColor;
        }

        // Mengaktifkan invincibility di script Health
        healthScript.SetInvincibility(true);

        yield return new WaitForSeconds(shieldDuration);

        // Mengembalikan kondisi awal
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        healthScript.SetInvincibility(false);
        shieldActive = false;
    }
}
