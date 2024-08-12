using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform[] spawnTransforms;
        public GameObject monsterPrefab;
    }

    [Header("Spawner Settings")]
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private float spawnInterval = 5.0f;
    [SerializeField] private float spawnDuration = 30.0f;
    [SerializeField] private float destroyCooldown = 5.0f;
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private bool isSpawning = false;
    private bool isCooldown = false;
    private float spawnTimer;
    private float durationTimer;
    private float cooldownTimer;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

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
                ActivateObject();

                if (cooldownText != null)
                {
                    cooldownText.text = "";
                }
            }
        }
    }

    private void SpawnMonsters()
    {
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnTransforms.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoint.spawnTransforms.Length);
                Transform chosenTransform = spawnPoint.spawnTransforms[randomIndex];

                GameObject spawnedMonster = Instantiate(spawnPoint.monsterPrefab, chosenTransform.position, chosenTransform.rotation);

                // Enable movement on the spawned monster
                EnemyPatrol enemyPatrol = spawnedMonster.GetComponent<EnemyPatrol>();
                if (enemyPatrol != null)
                {
                    enemyPatrol.canMove = true;
                }

                spawnedMonsters.Add(spawnedMonster);
            }
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
                Destroy(monster);
            }
        }
        spawnedMonsters.Clear();
    }

    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}
