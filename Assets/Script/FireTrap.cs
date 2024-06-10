using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activationTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the trap trigger.");
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Health component found on player.");
                if (!triggered)
                    StartCoroutine(ActivateFiretrap());

                if (active)
                    playerHealth.TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);  // Assuming you have a boolean parameter for activation
        yield return new WaitForSeconds(activationTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);  // Deactivate the trap
    }
}
