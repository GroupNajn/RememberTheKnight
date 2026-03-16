using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Managers/Event System")]
public class Event_System : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Action<EnemyDamage> OnEnemyKilled;
    public Action<EnemyDamage> OnEnemySpawn;
    public Action<int> OnPlayerDamaged;
    public Action<Droppable> OnGetLoot;


   

}
