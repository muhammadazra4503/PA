using UnityEngine;

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

    private bool isSpawning = false;
    private float spawnTimer;
    private float durationTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSpawning)
        {
            StartSpawning();
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
                SpawnMonsters();
                spawnTimer = spawnInterval;
            }

            if (durationTimer <= 0)
            {
                isSpawning = false;
            }
        }
    }

    private void SpawnMonsters()
    {
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            Instantiate(spawnPoint.monsterPrefab, spawnPoint.spawnTransform.position, spawnPoint.spawnTransform.rotation);
        }
    }
}
