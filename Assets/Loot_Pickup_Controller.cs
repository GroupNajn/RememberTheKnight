using System.Collections;
using Unity.AppUI.Redux;
using UnityEngine;

public class Loot_Pickup_Controller : MonoBehaviour
{
    Loot_Pickup_Controller instance;


    void Start()
    {
        instance = this;
        StartCoroutine(SoulFollowPullAll());
    }

    void Update()
    {
        
    }



   

    IEnumerator SoulFollowPullAll()
    {
        if (LootManager.instance != null && LootManager.instance.DroppedLoot.Count != 0)
        {
            yield return new WaitForSeconds(10f);
            Event_System.instance?.OnPullAllLoot.Invoke();
        }

    }

}
