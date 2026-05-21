using System;
using System.Collections.Generic;
using UnityEngine;


public class Event_System : MonoBehaviour
{
    public static Event_System instance;
    public Action OnSceneTransitionDone;
    public Action<CardFamily> OnContractSign;



    //Events 
    public Action OnWin;
    public Action OnLevelCompleted;

    //Player Related
    public Action OnPlayerDeath;
    public Action<int> OnPlayerDamaged;

    //Enemy Related
    public Action OnEnemiesSpawned;
    public Action<Transform, DamageInfo> OnEnemyDamage;
    public Action<EnemyDamage> OnEnemySpawn;
    public Action<EnemyLootProfile, Vector3> OnEnemyKilledNew;

    //UI & Shop Related
    public Action<List<CardData>> OnConfirmCardSelection;
    public Action<List<CardData>> OnConfirmPurchase;
    public Action<CardData> OnSacrificeSuccessful;

    //Loot Related
    public Action <CardData>OnDroopMultipleSouls;
    public Action OnResetPullAllLoot;
    public Action<Loot> OnLootPickedUp;
    public Action<Loot> OnGetLoot;
    public Action<IDamageable> OnSpawnChargedSoul;
    public Action<CardData> OnCardPickUp;

    //Souls Related
    public Action<int> OnSoulsSpent;
    public Action OnPullAllSouls;
    public Action OnResetSouls;

    //Scene Related
    public Action OnLoadScenes;
    public Action OnLobbyLoaded;

    //Boss Related
    public Action OnSpawnBoss;
    public Action OnBossDeath;

    // UI Related
    public Action OnDeviceChanged;

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
