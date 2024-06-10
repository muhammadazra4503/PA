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
        isInvincible = true;
        if (headSpriteRenderer != null)
        {
            headSpriteRenderer.color = invincibilityColor;
        }

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        if (headSpriteRenderer != null)
        {
            headSpriteRenderer.color = originalColor;
        }
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
}
