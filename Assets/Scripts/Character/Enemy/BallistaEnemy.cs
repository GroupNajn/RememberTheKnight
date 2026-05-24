using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallistaEnemy : MonoBehaviour
{
    // Condition for destroying ballista
    [Header("Enemies")]
    [SerializeField] int minRequiredEnemies = 4;
    [SerializeField] int maxRequiredEnemies = 7;
    private HashSet<GameObject> requiredEnemies = new HashSet<GameObject>();
    [SerializeField] private Vector3 soulOffset;
    int requiredEnemiesCount;
    [SerializeField] List<string> bannedEnemyNames = new List<string>();
    [SerializeField] List<string> bannedEnemyChildNames = new List<string>();

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
        //Debug.Log($"{name} is looking for enemies");
        GameObject[] enemiesFound = GameObject.FindGameObjectsWithTag("Enemy");

        requiredEnemiesCount = Random.Range(minRequiredEnemies, maxRequiredEnemies);

        //Debug.Log($"requiredEnemiesCount: {requiredEnemiesCount}, enemies found: {enemiesFound.Length}");

        for (int i = 0; i < requiredEnemiesCount && i < enemiesFound.Length; i++)
        {
            GameObject randomEnemy = null;
            int enemyToAddIndex;

            for (int j = 0; j < enemiesFound.Length; j++)
            {
                if (randomEnemy != null)
                {
                    //Debug.Log($"{name} found enemy {randomEnemy.name}");
                    break;
                }

                enemyToAddIndex = Random.Range(0, enemiesFound.Length);
                randomEnemy = enemiesFound[enemyToAddIndex];

                if (randomEnemy == null) continue;

                foreach (string name in bannedEnemyNames)
                {
                    if (randomEnemy.name.Contains(name))
                    {
                        //Debug.Log($"{randomEnemy.name} contains {name}");
                        randomEnemy = null;
                        break;
                    }
                }

                if (randomEnemy == null) continue;

                Transform[] children = randomEnemy.GetComponentsInChildren<Transform>();
                //Debug.Log($"{randomEnemy.name} has {children.Length} children");
                for (int k = 0; k < children.Length; k++)
                {
                    if (randomEnemy == null)
                    {
                        break;
                    }

                    foreach (string name in bannedEnemyChildNames)
                    {
                        if (children[k].name.Contains(name))
                        {
                            //Debug.Log($"Child contains {name}");
                            randomEnemy = null;
                            break;
                        }
                    }
                }

                if (randomEnemy != null)
                {
                    requiredEnemies.Add(enemiesFound[enemyToAddIndex]);
                    enemiesFound[enemyToAddIndex] = null;
                }
            }
        }

        //Debug.Log($"Scene: {gameObject.scene.name} requiredEnemiesCount: {requiredEnemiesCount} Enemies in requiredEnemies: {requiredEnemies.Count}, min required enemies: {minRequiredEnemies}, max required enemies: {maxRequiredEnemies}");

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

    private void OnDestroy()
    {
        Event_System.instance.OnEnemyKilledNew -= OnEnemyKilled;
        Event_System.instance.OnEnemiesSpawned -= OnEnemiesSpawned;
    }
}