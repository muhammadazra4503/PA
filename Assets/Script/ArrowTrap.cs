using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;
        
        int arrowsIndex = Findarrows();
        if (arrowsIndex >= 0)
        {
            arrows[arrowsIndex].transform.position = firePoint.position;
            arrows[arrowsIndex].SetActive(true);
            arrows[arrowsIndex].GetComponent<EnemyProjectile>().ActivateProjectile();
        }
    }

    private int Findarrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy) // Check if the GameObject is inactive
            {
                return i;
            }
        }
        return -1; // Return -1 if no inactive fireball is found
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }
}
