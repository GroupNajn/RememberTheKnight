using System.Collections.Generic;
using UnityEngine;

public class CrystalBallSpawner : MonoBehaviour
{
    [SerializeField] GameObject bossLevelCrystalBall;
    [SerializeField] List<GameObject> crystalBallPrefabs;
    [SerializeField] List<Transform> spawnPoints;

    private void Awake()
    {
        Event_System.instance.OnLevelCompleted += SpawnCrystalBalls;
    }

    private void SpawnCrystalBalls()
    {
        if (RunGameData.Instance.LevelCounter == RunGameData.Instance.LevelsBeforeBoss) // Only load boss level
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(bossLevelCrystalBall, spawnPoint.position, spawnPoint.rotation);
            }
        }
        else
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                GameObject prefab = crystalBallPrefabs[Random.Range(0, crystalBallPrefabs.Count)];
                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }

    private void OnDisable()
    {
        Event_System.instance.OnLevelCompleted -= SpawnCrystalBalls;
    }
}