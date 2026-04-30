using JetBrains.Annotations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


// Script made by Henric 2026-04-16
public class Loot_System : MonoBehaviour
{
    [field: SerializeField] public int currentSoulCount { get; private set; } = 0;
    [SerializeField] CardData[] selectedCards = new CardData[4];
    private Soul_Canvas_Text_Script canvasTextScript;
    private readonly Dictionary<int, Loot> soulsCollected = new Dictionary<int, Loot>();
    // Dictonary used to see if a souls has been collected before, to prevent a double event invoke from,
    // same soul not to trigger double souls_collected.
    private int id = 0;

    void Start()
    {


        if (Event_System.instance != null)
        {
            Event_System.instance.OnLootPickedUp += IncreaseSouls;
           
            Event_System.instance.OnSoulsSpent += ConsumeSouls;
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

        soulsCollected.Add(id, loot);
        id++;

        if (loot is Soul)
        {
            currentSoulCount += 1;
            canvasTextScript.SetSoulsAmount(currentSoulCount);
        }
        IncreaseCupCharges(loot);
    }

    private void IncreaseCupCharges(Loot loot)
    {
        if (loot.TryGetComponent<ChargedSoul>(out ChargedSoul chargedSoul))
        {

            PlayerManager p = GameObject.Find("Player").GetComponent<PlayerManager>();
            p.GetCharges(chargedSoul.ChargeAmount);
            Debug.Log("HEALING SOUL GOT COLLECTED!");
        }
    }
}
