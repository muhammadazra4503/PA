using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private Animator _animator;
    private bool _isPlayerInside;
    private Health _playerHealth;
    private EnemyPatrol _enemyPatrol;

    [SerializeField] private GameObject attackTrigger; // Assign the trigger GameObject in the inspector

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyPatrol = GetComponentInParent<EnemyPatrol>();  // Assuming MeleeEnemy is a child of the patrolling enemy
    }

    private void OnEnable()
    {
        // Subscribe to the trigger's events
        attackTrigger.GetComponent<AttackTrigger>().OnPlayerEnter += HandlePlayerEnter;
        attackTrigger.GetComponent<AttackTrigger>().OnPlayerStay += HandlePlayerStay;
        attackTrigger.GetComponent<AttackTrigger>().OnPlayerExit += HandlePlayerExit;
    }

    private void OnDisable()
    {
        // Unsubscribe from the trigger's events
        attackTrigger.GetComponent<AttackTrigger>().OnPlayerEnter -= HandlePlayerEnter;
        attackTrigger.GetComponent<AttackTrigger>().OnPlayerStay -= HandlePlayerStay;
        attackTrigger.GetComponent<AttackTrigger>().OnPlayerExit -= HandlePlayerExit;
    }

    private void HandlePlayerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            _playerHealth = other.GetComponent<Health>();
            if (_enemyPatrol != null)
            {
                _enemyPatrol.PausePatrol(); // Stop patrolling
            }
            _animator.SetTrigger("meleeAttack");
            Debug.Log("Player entered enemy attack trigger. Triggering melee attack.");
        }
    }

    private void HandlePlayerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetTrigger("meleeAttack");
        }
    }

    private void HandlePlayerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _animator.SetTrigger("idle");
            Debug.Log("Player exited enemy attack trigger. Returning to idle.");
            _playerHealth = null;
            if (_enemyPatrol != null)
            {
                _enemyPatrol.ResumePatrol(); // Resume patrolling
            }
        }
    }

    // Function to be called by the Animation Event
    public void OnMeleeAttack()
    {
        Debug.Log("Melee attack Animation Event triggered.");
        if (_isPlayerInside && _playerHealth != null)
        {
            _playerHealth.TakeDamage(1); // Assuming 1 damage per attack
        }
    }
}
