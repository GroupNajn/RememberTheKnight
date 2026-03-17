using System;
using UnityEngine;


public class Event_System : MonoBehaviour
{
    public static Event_System instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Action<EnemyDamage> OnEnemyKilled;
    public Action<EnemyDamage> OnEnemySpawn;
    public Action<int> OnPlayerDamaged;
    public Action<Droppable> OnGetLoot;

    /*
     * Managers need to be initialized via Awake to get priority,
     * before all other GameObjects call and Subscribe to their Actions/Events, 
     */


    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {

    }


}
