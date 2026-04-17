using System.Collections.Generic;
using UnityEngine;


// Script made by Henric 2026-04-16
public class Loot_System : MonoBehaviour
{
    [SerializeField] int Souls_Collected = 0;
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
        }
        canvasTextScript = GameObject.Find("Soul_Canvas").GetComponent<Soul_Canvas_Text_Script>();
    }

    private void OnDisable()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnLootPickedUp -= IncreaseSouls;
        }
        soulsCollected.Clear();
    }



    void Update()
    {

    }


    // a condition in the method to see if a soul has been collected before, to prevent double souls_collected
    // from the same soul. 
    public void IncreaseSouls(Loot loot)
    {
        if (soulsCollected.ContainsValue(loot)) return;

        soulsCollected.Add(id, loot);
        id++;

        if (loot.lootData == null)
        {
            Souls_Collected += 1;
            canvasTextScript.SetSoulsAmount(Souls_Collected);
            Debug.Log($"Souls Collected: {Souls_Collected}");
        }
    }
}
