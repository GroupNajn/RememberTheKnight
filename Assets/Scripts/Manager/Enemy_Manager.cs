using System.Collections.Generic;
using UnityEngine;


public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager instace;
    private HashSet<EnemyDamage> Enemies = new HashSet<EnemyDamage>();

    /*
     * Managers need to be initialized via Awake to get priority,
     * before all other GameObjects call and Subscribe to their Actions/Events, 
     */
    private void Awake()
    {
        instace = this;
    }

   

    //private void Start()
    //{
    //    if (Event_System.instance == null)
    //    {
    //        Debug.LogError("Event_System.instance is null in Enemy_Manager.Start()");
    //        return;
    //    }

    //    Event_System.instance.OnEnemySpawn += RegisterEnemy;
    //    Event_System.instance.OnEnemyKilled += RemoveEnemy;
    //}

    //private void OnDestroy()
    //{
    //    if (Event_System.instance != null)
    //    {
    //        Event_System.instance.OnEnemySpawn -= RegisterEnemy;
    //        Event_System.instance.OnEnemyKilled -= RemoveEnemy;
    //    }
    //}

    //public void RegisterEnemy(EnemyDamage enemy)
    //{
    //    Enemies.Add(enemy);
    //}

    //public void RemoveEnemy(EnemyDamage enemy)
    //{
    //    Enemies.Remove(enemy);
    //}

    //public IEnumerable<EnemyDamage> GetEnemies()
    //{
    //    return Enemies;
    //}
}
