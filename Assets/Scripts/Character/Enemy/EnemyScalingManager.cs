using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyScalingManager : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();
    List<EnemyWeaponManager> enemyWeaponManagers = new List<EnemyWeaponManager>();
    List<EnemyDamage> enemyDamages = new List<EnemyDamage>();

    private void Start()
    {
        Event_System.instance.OnEnemiesSpawned += EnemiesSpawned;
    }

    private void EnemiesSpawned()
    {
        enemies.Clear();
        enemyWeaponManagers.Clear();
        enemyDamages.Clear();

        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemyWeaponManagers = enemies.Select(e => e.GetComponent<EnemyWeaponManager>()).ToList();
        enemyDamages = enemies.Select(e => e.GetComponent<EnemyDamage>()).ToList();

        Debug.Log($"Found {enemies.Count} enemies in the scene.");
        foreach (EnemyDamage enemyDamage in enemyDamages)
        {
            if (enemyDamage != null)
            {
                enemyDamage.SetHealthModifier(RunGameData.Instance.LevelCounter);
            }
        }

        foreach (EnemyWeaponManager enemyWeaponManager in enemyWeaponManagers)
        {
            if (enemyWeaponManager != null)
            {
                enemyWeaponManager.SetDamageModifier(RunGameData.Instance.LevelCounter);
            }
        }
    }

    private void OnDestroy()
    {
        Event_System.instance.OnEnemiesSpawned += EnemiesSpawned;
    }
}