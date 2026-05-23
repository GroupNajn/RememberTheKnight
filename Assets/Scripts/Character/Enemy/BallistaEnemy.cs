using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaEnemy : MonoBehaviour
{
    // Condition for destroying ballista
    [Header("Enemies")]
    [SerializeField] int minRequiredEnemies = 4;
    [SerializeField] int maxRequiredEnemies = 7;
    private List<GameObject> requiredEnemies = new List<GameObject>();
    [SerializeField] private Vector3 soulOffset;
    int requiredEnemiesCount;
    [SerializeField] List<string> bannedEnemyNames = new List<string>();

    [Header("Ballista Specifics")]
    private HashSet<GameObject> barrels = new HashSet<GameObject>();
    [SerializeField] private GameObject destroyedBallista;
    [SerializeField] private GameObject ballistaSoul;
    [SerializeField] private bool IsDestroyed = false;
    public EventReference ballistaShootEvent;
    public EventReference ballistaLoadEvent;

    void Start()
    {
        Event_System.instance.OnEnemyKilledNew += OnEnemyKilled;
        Event_System.instance.OnEnemiesSpawned += OnEnemiesSpawned;

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name.Contains("ExplosiveBarrel")) barrels.Add(child.gameObject);
        }
    }

    private void OnEnemiesSpawned()
    {
        GameObject[] enemiesFound = GameObject.FindGameObjectsWithTag("Enemy");

        requiredEnemiesCount = Random.Range(minRequiredEnemies, maxRequiredEnemies + 1);

        for (int i = 0; i < requiredEnemiesCount; i++)
        {
            GameObject randomEnemy;
            int enemyToAddIndex;

            do
            {
                enemyToAddIndex = Random.Range(0, enemiesFound.Length);
                randomEnemy = enemiesFound[enemyToAddIndex];

                if (randomEnemy == null) continue;

                foreach (string name in bannedEnemyNames)
                {
                    if (randomEnemy.name.Contains(name))
                    {
                        randomEnemy = null;
                        Debug.Log($"Random enemy name contains {name}");
                        break;
                    }
                }
            } while (randomEnemy == null);

            requiredEnemies.Add(enemiesFound[enemyToAddIndex]);
            enemiesFound[enemyToAddIndex] = null;
        }

        foreach (GameObject e in requiredEnemies)
        {
            GameObject soul = Instantiate(ballistaSoul, e.transform.position + soulOffset, Quaternion.identity, e.transform);
        }

    }

    void OnEnemyKilled(EnemyLootProfile enemy, Vector3 spawnPos)
    {
        if (IsDestroyed) return;

        if (requiredEnemies.Contains(enemy.gameObject))
        {
            requiredEnemies.Remove(enemy.gameObject);
            foreach (Transform child in enemy.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Contains("Ballista_Soul")) Destroy(child.gameObject);
            }
        }

        if (requiredEnemies.Count == 0)
        {
            StartCoroutine(ExplodeBarrels());
        }
    }

    IEnumerator ExplodeBarrels()
    {
        foreach (GameObject barrel in barrels)
        {
            barrel.GetComponent<ExplodingBarrel>().Explode();
            RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.explosionEvent, barrel);
            yield return new WaitForSeconds(0.1f);
        }

        Instantiate(destroyedBallista, transform.position, Quaternion.identity);
        IsDestroyed = true;
        Destroy(gameObject);
    }
}
