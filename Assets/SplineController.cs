using UnityEngine;
using UnityEngine.Splines;
using System.Collections;
using System.Collections.Generic;

public class SplineController : MonoBehaviour
{
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private List<SplineContainer> splines = new List<SplineContainer>();

    [Header("Spawn intervall")]
    [SerializeField] private float minSpawnTime = 10f;
    [SerializeField] private float maxSpawnTime = 15f;

    [Header("Move duration")]
    [SerializeField] private float minMoveDuration = 5f;
    [SerializeField] private float maxMoveDuration = 10f;
//Made 2026-05-19 Made by Henric
    private void Start()
    {
        StartCoroutine(SpawnSoulsRoutine());
    }

    private IEnumerator SpawnSoulsRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnSoul();
        }
    }
    /// <summary>
    /// Spawns a new soul GameObject and initializes its movement along a randomly selected spline.
    /// </summary>
    /// <remarks>Does nothing if the soul prefab is not assigned or if there are no available splines. The
    /// spawned soul is assigned a random movement duration within the configured range and is ensured to have a
    /// DonationSoulMover component for movement initialization.</remarks>
    private void SpawnSoul()
    {
        if (soulPrefab == null || splines.Count == 0)
            return;

        SplineContainer randomSpline = splines[Random.Range(0, splines.Count)];

        GameObject soul = Instantiate(soulPrefab);

        DonationSoulMover mover = soul.GetComponent<DonationSoulMover>();

        if (mover == null)
            mover = soul.AddComponent<DonationSoulMover>();

        float randomMoveDuration = Random.Range(minMoveDuration, maxMoveDuration);

        mover.Initialize(randomSpline, randomMoveDuration);
    }
}