using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.ExampleScripts;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth = 3f;
    public float CurrentHealth { get; private set; }

    [SerializeField] private Image[] healthBars;
    [SerializeField] private float knockbackForce = 5f;

    private Animator anim;
    private CharacterControls characterControls;
    private Rigidbody rb;
    private bool isDead = false;

    [SerializeField] private float invincibilityDuration = 2f;
    private bool isInvincible = false;
    [SerializeField] private Color invincibilityColor = Color.red;
    private Color originalColor;
    [SerializeField] private SpriteRenderer headSpriteRenderer;

    private UIManager uiManager;
    
    private bool isShieldActive = false; // Menyimpan status shield
    private Color shieldColor = Color.blue; // Warna saat shield aktif

    private void Awake()
    {
        CurrentHealth = startingHealth;
        UpdateHealthUI();
        anim = GetComponent<Animator>();
        characterControls = GetComponent<CharacterControls>();
        rb = GetComponent<Rigidbody>();

        if (headSpriteRenderer != null)
        {
            originalColor = headSpriteRenderer.color;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, startingHealth);
        UpdateHealthUI();

        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        ApplyKnockback();

        if (CurrentHealth <= 0)
        {
            Dead();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        SetInvincibility(true);
        yield return new WaitForSeconds(invincibilityDuration);
        SetInvincibility(false);
    }

    private void ApplyKnockback()
    {
        if (rb != null)
        {
            Vector3 knockbackDirection = -transform.forward;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, startingHealth);
        UpdateHealthUI();

        if (anim != null)
        {
            anim.SetTrigger("Heal");
        }
    }

    public void PickupHealth()
    {
        Heal(1);
    }

    private void Dead()
    {
        if (isDead) return;

        isDead = true;
        if (anim != null)
        {
            anim.SetTrigger("Dead");
        }

        if (characterControls != null)
        {
            characterControls.enabled = false;
        }

        Debug.Log("Player is dead.");

        // Call Respawn after a delay to simulate a death animation or delay
        Invoke("Respawn", 2f);  // Adjust the delay as needed
    }

    private void Respawn()
    {
        if (characterControls != null)
        {
            characterControls.Respawn();
            isDead = false;
            characterControls.enabled = true;
        }
    }

    public void ResetHealth()
    {
        CurrentHealth = startingHealth;
        UpdateHealthUI();
        isDead = false;
        isInvincible = false;
        isShieldActive = false; // Reset shield status
        if (headSpriteRenderer != null)
        {
            headSpriteRenderer.color = originalColor;
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].enabled = i < CurrentHealth;
        }
    }

    private void Update()
    {
        // Other update logic if needed
    }

    public void SetInvincibility(bool state)
    {
        isInvincible = state;
        UpdateSpriteColor();
    }

    public void ActivateShield(bool state)
    {
        isShieldActive = state;
        UpdateSpriteColor();
    }

    private void UpdateSpriteColor()
    {
        if (headSpriteRenderer != null)
        {
            if (isShieldActive)
            {
                headSpriteRenderer.color = shieldColor; // Warna biru jika shield aktif
            }
            else if (isInvincible)
            {
                headSpriteRenderer.color = invincibilityColor; // Warna merah jika invincible aktif
            }
            else
            {
                headSpriteRenderer.color = originalColor; // Warna asli jika tidak ada shield atau invincibility
            }
        }
    }
}
