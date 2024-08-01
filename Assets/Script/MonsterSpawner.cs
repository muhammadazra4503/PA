using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform spawnTransform; // Titik spawn
        public GameObject monsterPrefab; // Prefab monster yang akan di-spawn di titik spawn ini
    }

    [Header("Spawner Settings")]
    [SerializeField] private SpawnPoint[] spawnPoints; // Array titik spawn dan prefab
    [SerializeField] private float spawnInterval = 5.0f; // Interval waktu antara spawn monster
    [SerializeField] private float spawnDuration = 30.0f; // Durasi total spawn monster
    [SerializeField] private float destroyCooldown = 5.0f; // Waktu cooldown sebelum monster dihancurkan
    [SerializeField] private GameObject objectToActivate; // Game object yang akan diaktifkan

    private bool isSpawning = false; // Apakah sistem sedang melakukan spawn
    private bool isCooldown = false; // Apakah dalam mode cooldown
    private float spawnTimer; // Timer untuk interval spawn
    private float durationTimer; // Timer untuk durasi total spawn
    private float cooldownTimer; // Timer untuk cooldown sebelum destroy
    private List<GameObject> spawnedMonsters = new List<GameObject>(); // Daftar monster yang telah di-spawn

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSpawning)
        {
            StartSpawning(); // Mulai spawn monster ketika pemain masuk trigger
        }
    }

    private void StartSpawning()
    {
        isSpawning = true;
        spawnTimer = spawnInterval;
        durationTimer = spawnDuration;
    }

    private void Update()
    {
        if (isSpawning)
        {
            spawnTimer -= Time.deltaTime;
            durationTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                SpawnMonsters(); // Spawn monster setiap interval waktu tertentu
                spawnTimer = spawnInterval;
            }

            if (durationTimer <= 0 && !isCooldown)
            {
                isSpawning = false;
                StartCooldown(); // Mulai cooldown ketika durasi total habis
            }
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                DestroyAllSpawnedMonsters(); // Hancurkan semua monster setelah cooldown
                isCooldown = false;
                ActivateObject(); // Aktifkan game object setelah semua monster dihancurkan
            }
        }
    }

    private void SpawnMonsters()
    {
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            GameObject spawnedMonster = Instantiate(spawnPoint.monsterPrefab, spawnPoint.spawnTransform.position, spawnPoint.spawnTransform.rotation);
            spawnedMonsters.Add(spawnedMonster);
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = destroyCooldown;
    }

    private void DestroyAllSpawnedMonsters()
    {
        foreach (GameObject monster in spawnedMonsters)
        {
            if (monster != null)
            {
                Destroy(monster); // Hancurkan monster yang masih ada
            }
        }
        spawnedMonsters.Clear(); // Kosongkan daftar monster yang telah di-spawn
    }

    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true); // Aktifkan game object
        }
    }
}
