using System.Collections;
using System.Collections.Generic; // Required for using List
using UnityEngine;

public class EnemyHealthWithShield : MonoBehaviour
{
    [SerializeField] private float startingHealth = 3f;
    public float CurrentHealth { get; private set; }

    [SerializeField] private Color hitColor = Color.red;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead = false;

    [SerializeField] private Collider damageCollider;  // Assign this in the Inspector

    // Shield related variables
    [SerializeField] private GameObject shieldObject; // GameObject for the shield
    [SerializeField] private List<GameObject> objectsToDestroy; // List of GameObjects that must be destroyed to deactivate the shield

    private bool isShieldActive = false;

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

        if (shieldObject == null)
        {
            Debug.LogError("ShieldObject is not assigned. Please assign it in the Inspector.");
        }

        if (objectsToDestroy == null || objectsToDestroy.Count == 0)
        {
            Debug.LogError("ObjectsToDestroy list is empty. Please assign the objects in the Inspector.");
        }

        shieldObject.SetActive(false); // Ensure the shield is initially inactive
    }

    private void Update()
    {
        // Activate the shield if health drops to 1 and the shield is not active
        if (CurrentHealth <= 1 && !isShieldActive)
        {
            ActivateShield();
        }

        // Deactivate the shield if all objects in the list are destroyed
        if (isShieldActive && AllObjectsDestroyed())
        {
            DeactivateShield();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        // Check if the shield is active
        if (isShieldActive)
        {
            Debug.Log("Damage blocked by shield.");
            return; // No damage taken if the shield is active
        }

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

    // Shield management methods
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

    // Check if all objects in the list are destroyed
    private bool AllObjectsDestroyed()
    {
        // Return true if all objects in the list are null (i.e., destroyed)
        return objectsToDestroy.TrueForAll(obj => obj == null);
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}
