using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Made by Lukas 2026-03-24
    // Updated by Lukas 2026-04-24
    [SerializeField] List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] float spawnRadius = 10f; // Used if spawnPoints is empty
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] bool randomObjectSpawns = false;

    bool randomSpawnPoints;
    int currentSpawnIndex = 0;

    void Start()
    {
        randomSpawnPoints = spawnPoints.Count == 0;

        if (randomObjectSpawns)
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
            Vector3 spawnPosition;

            if (randomSpawnPoints)
            {
                float randomX = Random.Range(-spawnRadius, spawnRadius);
                float randomZ = Random.Range(-spawnRadius, spawnRadius);

                spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);
            }
            else
            {
                spawnPosition = spawnPoints[currentSpawnIndex].position;
                currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Count;
            }

            Instantiate(spawnObjects[i], spawnPosition, Quaternion.identity);
        }
    }

    public void RandomSpawn()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Vector3 spawnPosition;

            GameObject randomObject = spawnObjects[Random.Range(0, spawnObjects.Count)];

            if (randomSpawnPoints)
            {
                float randomX = Random.Range(-spawnRadius, spawnRadius);
                float randomZ = Random.Range(-spawnRadius, spawnRadius);

                spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);
            }
            else
            {
                spawnPosition = spawnPoints[i].position;
            }

            Instantiate(randomObject, spawnPosition, Quaternion.identity);
        }
    }
}