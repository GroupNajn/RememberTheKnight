using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Made by Lukas 2026-03-24
    // Updated by Lukas 2026-04-24
    [SerializeField] bool isEnemySpawner = true;
    [SerializeField] bool drawGizmos = true;
    [SerializeField] Color gizmoColor = Color.red;
    [SerializeField] List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] float spawnRadius = 10f; // Used if spawnPoints is empty
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] bool randomObjectSpawns = false;
    [SerializeField, Range(0, 1)] float spawnChance; 

    bool randomSpawnPoints;
    int currentSpawnIndex = 0;
    bool spawnAtStart = true;

    private void Awake()
    {
        // Subscribe to the OnLoadScenes event to trigger spawning when scenes are loaded
        if (Event_System.instance)
        {
            Event_System.instance.OnLoadScenes += InitializeSpawn;

            spawnAtStart = false;
        }
    }

    private void Start()
    {
        if (spawnAtStart)
        {
            InitializeSpawn();
        }
    }

    void InitializeSpawn()
    {
        randomSpawnPoints = spawnPoints.Count == 0;

        if (randomSpawnPoints)
        {
            Spawn();
        }
        else if (randomObjectSpawns)
        {
            RandomSpawn();
        }
        else
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnObjects.Count; i++)
        {
            if (spawnObjects[i] == null)
            {
                continue;
            }

            if (Random.Range(0f, 1f) > spawnChance)
            {
                continue;
            }

            Vector3 spawnPosition;
            Quaternion spawnRotation = Quaternion.identity;

            if (randomSpawnPoints)
            {
                float randomX = Random.Range(-spawnRadius, spawnRadius);
                float randomZ = Random.Range(-spawnRadius, spawnRadius);

                spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);
            }
            else
            {
                if (!spawnPoints[currentSpawnIndex])
                {
                    continue;
                }

                spawnPosition = spawnPoints[currentSpawnIndex].position;
                spawnRotation = spawnPoints[currentSpawnIndex].rotation;
                currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Count;
            }

            Instantiate(spawnObjects[i], spawnPosition, spawnRotation);
        }

        Debug.Log($"{name}'s isEnemySpawner: {isEnemySpawner}");

        if (isEnemySpawner)
        {
            Debug.Log($"Enemies spawned by {name}");
            StartCoroutine(WaitToInvokeEnemySpawned());
        }
    }

    public void RandomSpawn()
    {
        foreach (Transform spawnPosition in spawnPoints)
        {
            if (!spawnPosition)
            {
                continue;
            }

            if (Random.Range(0f, 1f) > spawnChance)
            {
                continue;
            }

            GameObject randomObject = spawnObjects[Random.Range(0, spawnObjects.Count)];

            if (randomObject == null)
            {
                continue;
            }

            Instantiate(randomObject, spawnPosition.position, spawnPosition.rotation);
        }

        if (isEnemySpawner)
        {
            Debug.Log($"Enemies spawned by {name}");
            StartCoroutine(WaitToInvokeEnemySpawned());
        }
    }

    IEnumerator WaitToInvokeEnemySpawned()
    {
        yield return null;
        Event_System.instance.OnEnemiesSpawned?.Invoke();
    }

    private void OnDestroy()
    {
        Event_System.instance.OnLoadScenes -= InitializeSpawn;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }

        Gizmos.color = gizmoColor;
        if (spawnPoints.Count > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    Gizmos.DrawSphere(spawnPoint.position, 0.1f);
                }
            }
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadius * 2, 0f, spawnRadius * 2));
        }
    }
}