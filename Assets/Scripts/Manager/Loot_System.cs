using JetBrains.Annotations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


// Script made by Henric 2026-04-16
public class Loot_System : MonoBehaviour
{
    [field: SerializeField] public int currentSoulCount { get; private set; } = 0;
    private Soul_Canvas_Text_Script canvasTextScript;
    private readonly Dictionary<int, Loot> soulsCollected = new Dictionary<int, Loot>();
    private float timeSinceLastSoulCollected = 0f;
    public float soulSoundCollectionReset = 5f; //seconds
    private bool checkForSoundReset = false;
    // Dictonary used to see if a souls has been collected before, to prevent a double event invoke from,
    // same soul not to trigger double souls_collected.
    private int id = 0;

    void Start()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnLootPickedUp += IncreaseSouls;
           
            Event_System.instance.OnSoulsSpent += ConsumeSouls;

            Event_System.instance.OnResetSouls += ResetSouls;
        }
        canvasTextScript = GameObject.Find("Soul_Canvas").GetComponent<Soul_Canvas_Text_Script>();
    }

    private void OnDisable()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnLootPickedUp -= IncreaseSouls;
            Event_System.instance.OnSoulsSpent -= ConsumeSouls;
           
        }
        soulsCollected.Clear();
    }
    
    void Update()
    {
        if(checkForSoundReset)
        {
            timeSinceLastSoulCollected += Time.deltaTime;
            if(timeSinceLastSoulCollected >= soulSoundCollectionReset)
            {
                checkForSoundReset = false;
                timeSinceLastSoulCollected = 0f;
                GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters(); // trigger parameter to reset the soul collection pitch change
            }
        }
    }

    public void ConsumeSouls(int souls)
    {
        currentSoulCount -= souls;
        canvasTextScript.SetSoulsAmount(currentSoulCount);
    }


    // a condition in the method to see if a soul has been collected before, to prevent double souls_collected
    // from the same soul. 
    public void IncreaseSouls(Loot loot)
    {
        if (soulsCollected.ContainsValue(loot)) return;
        checkForSoundReset = true;
        timeSinceLastSoulCollected = 0f;
        soulsCollected.Add(id, loot);
        id++;

        if (loot is Soul)
        {
            Soul soul = (Soul)loot;
            currentSoulCount += soul.SoulCollectReward;
            canvasTextScript.SetSoulsAmount(currentSoulCount);
        }
        IncreaseCupCharges(loot);
    }

    private void IncreaseCupCharges(Loot loot)
    {
        if (loot.TryGetComponent<HealingSoul>(out HealingSoul healingSoul))
        {

            PlayerManager p = GameObject.Find("Player").GetComponent<PlayerManager>();
            p.GetCharges(healingSoul.HealingChargeAmount);
        }
    }

    void ResetSouls()
    {
        currentSoulCount = 0;
        canvasTextScript.SetSoulsAmount(currentSoulCount);
    }
}
