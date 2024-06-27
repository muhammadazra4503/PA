using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private Animator animator; // Tambahkan referensi ke Animator
    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;

        int arrowIndex = FindArrow();
        if (arrowIndex >= 0)
        {
            arrows[arrowIndex].transform.position = firePoint.position;
            arrows[arrowIndex].SetActive(true);
            arrows[arrowIndex].GetComponent<EnemyProjectile>().ActivateProjectile(); // Panggil metode tanpa parameter

            // Trigger animasi rangedAttack
            animator.SetTrigger("rangedAttack");
        }
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy) // Periksa apakah GameObject tidak aktif
            {
                return i;
            }
        }
        return -1; // Kembalikan -1 jika tidak ada panah yang tidak aktif
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
