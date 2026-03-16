using System.Collections.Generic;
using UnityEngine;


public class Enemy_Manager : MonoBehaviour 
{
    public static Enemy_Manager instace;
    [SerializeField] Event_System EventSystem;
    private HashSet<EnemyDamage> Enemies = new HashSet<EnemyDamage>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    private void OnEnable()
    {
        EventSystem.OnEnemySpawn += RegisterEnemy;
        EventSystem.OnEnemyKilled += RemoveEnemy;
    }

    private void OnDisable()
    {
        EventSystem.OnEnemySpawn -= RegisterEnemy;
        EventSystem.OnEnemyKilled -= RemoveEnemy;
    }


    public void RegisterEnemy(EnemyDamage enemy)
    {
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyDamage enemy)
    {
        Enemies.Remove(enemy);
    }

    public IEnumerable<EnemyDamage> GetEnemies()
    {
        return Enemies;
    }

   
}
