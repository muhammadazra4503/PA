using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth = 3f;
    public float CurrentHealth { get; private set; }

    [SerializeField] private Color hitColor = Color.red;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead = false;

    [SerializeField] private Collider damageCollider;  // Assign this in the Inspector

    private void Awake()
    {
        CurrentHealth = startingHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (damageCollider == null)
        {
            Debug.LogError("Damage collider is not assigned. Please assign it in the Inspector.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, startingHealth);
        StartCoroutine(FlashHitColor());

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashHitColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("die");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This method can be called at the end of the death animation
    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile != null)
            {
                TakeDamage(1); // Adjust damage value as needed
                projectile.OnHit(); // Disable the projectile or perform other actions
            }
        }
    }
}
