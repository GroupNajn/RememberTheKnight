using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/Enemy Manager")]

public class Enemy_Manager : ScriptableObject 
{
    [SerializeField] Event_System EventSystem;
    private HashSet<EnemyDamage> Enemies = new HashSet<EnemyDamage>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    private void OnEnable()
    {
        EventSystem.OnEnemySpawn += RegisterEnemy;
        EventSystem.OnEnemyKilled += RemoveEnemy;
    }

    private void OnDisable()
    {
        
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
