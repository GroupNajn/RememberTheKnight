using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Event_System : MonoBehaviour
{
    public static Event_System instance;
    public Action<EnemyDamage> OnEnemyKilled;
    public Action<EnemyDamage> OnEnemySpawn;
    public Action<int> OnPlayerDamaged;
    public Action<Loot> OnGetLoot;
    public Action<Transform, float> OnEnemyDamage;
    public Action<Loot> OnLootPickedUp;
    public Action OnPullAllLoot;
    public Action OnResetPullAllLoot;
    public Action OnPlayerDeath;
    public Action OnWin;
    public Action<List<CardData>> OnStatsApplied;
    public Action<int> OnSoulsSpent;

    /*
     * 
     * Managers need to be initialized via Awake to get priority,
     * before all other GameObjects call and Subscribe to their Actions/Events, 
     */


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate manager destroyed");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
