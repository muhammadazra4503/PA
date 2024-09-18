using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnTransform
    {
        public Transform transform;
        public float spawnInterval; // Individual spawn interval for this transform
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public SpawnTransform[] spawnTransforms; // Updated to use SpawnTransform
        public GameObject monsterPrefab;
    }

    [Header("Spawner Settings")]
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private float spawnDuration = 30.0f;
    [SerializeField] private float destroyCooldown = 5.0f;
    [SerializeField] private List<GameObject> objectsToActivate;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private bool isSpawning = false;
    private bool isCooldown = false;
    private float durationTimer;
    private float cooldownTimer;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private Dictionary<SpawnTransform, float> spawnTimers = new Dictionary<SpawnTransform, float>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isSpawning)
            {
                StartSpawning();
            }

            if (!isCooldown)
            {
                StartCooldown();
            }
        }
    }

    private void StartSpawning()
    {
        isSpawning = true;
        durationTimer = spawnDuration;

        // Initialize spawn timers for each spawn transform
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            foreach (SpawnTransform spawnTransform in spawnPoint.spawnTransforms)
            {
                spawnTimers[spawnTransform] = spawnTransform.spawnInterval; // Set the timer to the specified spawn interval
            }
        }
    }

    private void Update()
    {
        if (isSpawning)
        {
            durationTimer -= Time.deltaTime;

            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                foreach (SpawnTransform spawnTransform in spawnPoint.spawnTransforms)
                {
                    spawnTimers[spawnTransform] -= Time.deltaTime;

                    if (spawnTimers[spawnTransform] <= 0)
                    {
                        SpawnMonster(spawnPoint, spawnTransform);
                        spawnTimers[spawnTransform] = spawnTransform.spawnInterval; // Reset the timer
                    }
                }
            }

            if (durationTimer <= 0 && !isCooldown)
            {
                isSpawning = false;
            }
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownText != null)
            {
                cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
            }

            if (cooldownTimer <= 0)
            {
                DestroyAllSpawnedMonsters();
                isCooldown = false;
                ActivateObjects();

                if (cooldownText != null)
                {
                    cooldownText.text = "";
                }
            }
        }
    }

    private void SpawnMonster(SpawnPoint spawnPoint, SpawnTransform spawnTransform)
    {
        GameObject spawnedMonster = Instantiate(spawnPoint.monsterPrefab, spawnTransform.transform.position, spawnTransform.transform.rotation);

        // Enable movement on the spawned monster
        EnemyPatrol enemyPatrol = spawnedMonster.GetComponent<EnemyPatrol>();
        if (enemyPatrol != null)
        {
            enemyPatrol.canMove = true;
        }

        spawnedMonsters.Add(spawnedMonster);
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
                Destroy(monster);
            }
        }
        spawnedMonsters.Clear();
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
