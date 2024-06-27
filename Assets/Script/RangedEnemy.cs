using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private Animator _animator;
    private bool _isPlayerInside;
    private Health _playerHealth;

    public GameObject projectilePrefab; // Assign your projectile prefab in the inspector
    public Transform projectileSpawnPoint; // The point where the projectile will be instantiated
    public float attackCooldown = 1.0f; // Time between attacks
    private float _lastAttackTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            _playerHealth = other.GetComponent<Health>();
            _animator.SetTrigger("rangedAttack");
            Debug.Log("Player entered enemy collider. Triggering ranged attack.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time > _lastAttackTime + attackCooldown)
        {
            _lastAttackTime = Time.time;
            _animator.SetTrigger("rangedAttack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _animator.SetTrigger("idle");
            Debug.Log("Player exited enemy collider. Returning to idle.");
            _playerHealth = null;
        }
    }

    // Function to be called by the Animation Event
    public void OnRangedAttack()
    {
        Debug.Log("Ranged attack Animation Event triggered.");
        if (_isPlayerInside)
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
        if (projectileScript != null)
        {
            projectileScript.ActivateProjectile();
            projectile.transform.forward = transform.forward; // Set the projectile direction based on enemy facing
        }
    }
}