using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Made by Lukas 2026-03-24
    [SerializeField] List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] float spawnRadius = 10f; // Used if spawnPoints is empty
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();

    bool randomSpawnPoints;
    int currentSpawnIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomSpawnPoints = spawnPoints.Count == 0;

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {

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

            Debug.Log("Spawn position = " + spawnPosition);

            Instantiate(spawnObjects[i], spawnPosition, Quaternion.identity);
        }
    }
}