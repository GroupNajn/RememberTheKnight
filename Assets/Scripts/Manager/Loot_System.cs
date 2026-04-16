using UnityEngine;


// Script made by Henric 2026-04-16
public class Loot_System : MonoBehaviour
{
    [SerializeField] int Souls_Collected = 0;
    [SerializeField] CardData[] selectedCards = new CardData[4];
    private Soul_Canvas_Text_Script canvasTextScript;

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
    }



    void Update()
    {

    }



    public void IncreaseSouls(Loot loot)
    {
        if (loot.lootData == null)
        {
            Souls_Collected += 1;
            canvasTextScript.SetSoulsAmount(Souls_Collected);
            Debug.Log($"Souls Collected: {Souls_Collected}");
        }
    }



}
