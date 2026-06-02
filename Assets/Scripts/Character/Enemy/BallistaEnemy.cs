using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaEnemy : MonoBehaviour
{
    /// <summary>
    /// Created by Anton 2026-4-17
    /// Initially created with a aim at and shoot player function, which was later refactored into new scripts.
    /// 
    /// Refactored by Anton 2026-04-24
    /// Changed to have a new mechanic where the player has to kill a certain amount of enemies to destroy the ballista.
    /// Also moved logic about shooting to new scripts.
    /// 
    /// Changed by Anton and Lukas 2026-05-11
    /// Added visual effects on enemies in the form of a soul that indicates that they are required to destroy the ballista.
    /// </summary>
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


    // Find random enemies to set as required to destry ballista
    private void OnEnemiesSpawned()
    {
        //Debug.Log($"{name} is looking for enemies");
        GameObject[] enemiesFound = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies

        requiredEnemiesCount = Random.Range(minRequiredEnemies, maxRequiredEnemies + 1); // Get a random number as the amount of enemies required to destroy

        //Debug.Log($"requiredEnemiesCount: {requiredEnemiesCount}, enemies found: {enemiesFound.Length}");

        for (int i = 0; i < requiredEnemiesCount && i < enemiesFound.Length; i++)
        {
            GameObject randomEnemy = null;
            int enemyToAddIndex;

            for (int j = 0; j < enemiesFound.Length; j++)
            {
                if (randomEnemy != null)
                {
                    // Random enemy has already been found, look for next random enemy
                    //Debug.Log($"{name} found enemy {randomEnemy.name}");
                    break;
                }

                // Get a random enemy
                enemyToAddIndex = Random.Range(0, enemiesFound.Length);
                randomEnemy = enemiesFound[enemyToAddIndex];

                if (randomEnemy == null) continue; // Random enemy has already been set as required enemy and we don't need to check for name match

                foreach (string name in bannedEnemyNames)
                {
                    if (randomEnemy.name.Contains(name))
                    {
                        // If the enemy's name contains a banned name don't add to required enemies, mainly used to avoid having ballistas as required enemies
                        //Debug.Log($"{randomEnemy.name} contains {name}");
                        randomEnemy = null;
                        break;
                    }
                }

                if (randomEnemy == null) continue; // Random enemy has failed name check and we don't need to check for child name match

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
                            // If any if the cild objects of the enemy contains a banned child name it will ignore it, mainly used to check if an enemy is required by another ballista
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
            // Spawn the soul indicating the enemy is a required enemy on the enemy
            GameObject soul = Instantiate(ballistaSoul, e.transform.position + soulOffset, Quaternion.identity, e.transform);
        }
    }

    // Remove enemies after from requierment list when it's killed
    void OnEnemyKilled(EnemyLootProfile enemy, Vector3 spawnPos)
    {
        if (IsDestroyed) return;

        if (requiredEnemies.Contains(enemy.gameObject))
        {
            // Remove enemy from the requerement list if it exists in the list
            requiredEnemies.Remove(enemy.gameObject);
            foreach (Transform child in enemy.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Contains("Ballista_Soul")) Destroy(child.gameObject);
            }
        }

        if (requiredEnemies.Count == 0)
        {
            // All enemies killed, explode
            StartCoroutine(ExplodeBarrels());
        }
    }

    IEnumerator ExplodeBarrels()
    {
        foreach (GameObject barrel in barrels)
        {
            // Explode all the barrels with a 0.1s interval
            barrel.GetComponent<ExplodingBarrel>().Explode();
            RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.explosionEvent, barrel);
            yield return new WaitForSeconds(0.1f);
        }

        // Spawn the destroyed ballista prefab
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