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

            //Event_System.instance.OnBossDeath += EnableBossPedestal;
        }
        canvasTextScript = GameObject.Find("Soul_Canvas").GetComponent<Soul_Canvas_Text_Script>();
    }

    private void OnDisable()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnLootPickedUp -= IncreaseSouls;
            Event_System.instance.OnSoulsSpent -= ConsumeSouls;
            Event_System.instance.OnResetSouls -= ResetSouls;
            //Event_System.instance.OnBossDeath -= EnableBossPedestal;

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
    //private void EnableBossPedestal()
    //{
    //    GameObject bossCardPedestal = GameObject.Find("Boss_Pedestal");
    //    bossCardPedestal.SetActive(true);
        
    //}

    public void ConsumeSouls(int souls)
    {
        currentSoulCount -= souls;
        canvasTextScript.SetSoulsAmount(currentSoulCount);
    }


    // a condition in the method to see if a soul has been collected before, to prevent double souls_collected
    // from the same soul. 
    /// <summary>
    /// Adds the specified loot to the collection of collected souls and updates the soul count if the loot is a soul.
    /// </summary>
    /// <remarks>If the specified loot has already been collected, this method does not add it again or update
    /// the soul count. This prevents duplicate collection of the same soul.</remarks>
    /// <param name="loot">The loot item to be collected. If the loot is of type Soul, its reward value is added to the current soul count.
    /// Cannot be null.</param>
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
    /// <summary>
    /// Increases the player's healing cup charges based on the specified loot item, if it contains a HealingSoul
    /// component.
    /// </summary>
    /// <param name="loot">The loot item to check for a HealingSoul component. If present, its healing charge amount is added to the
    /// player's charges.</param>
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
