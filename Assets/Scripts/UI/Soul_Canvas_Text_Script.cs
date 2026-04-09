using UnityEngine;
using TMPro;
public class Soul_Canvas_Text_Script : MonoBehaviour
{
    int soulAmount = 0;
    TextMeshProUGUI tmp;

    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        Event_System.instance.OnLootPickedUp += IncreaseSoulAmount;
        tmp.text = soulAmount.ToString();
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void OnDisable()
    {
        Event_System.instance.OnLootPickedUp -= IncreaseSoulAmount;
    }


    void Update()
    {
        
    }

    private void IncreaseSoulAmount()
    {
        soulAmount++;
        tmp.text = soulAmount.ToString();
    }

    
}
