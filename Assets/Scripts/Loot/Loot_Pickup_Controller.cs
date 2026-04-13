using System.Collections;
using UnityEngine;

public class LootFollowController : MonoBehaviour
{
    //Script made by Henric 2026-04-01

    [SerializeField] private float overrideDelay = 10f;

    private bool overridden = false;
    private bool overrideCoroutineRunning = false;
    private bool manaulOverride;
    private Coroutine overrideRoutine;

    private void Update()
    {
        if (LootManager.instance == null)
            return;

        bool hasLootThreshhold = LootManager.instance.DroppedLoot.Count > 1;

        if (Input.GetKeyDown(KeyCode.H) && hasLootThreshhold)
        {
            Event_System.instance.OnPullAllLoot?.Invoke();
            overridden = true;
        }


        // Start override routine 
        //if (!overridden && !overrideCoroutineRunning)
        //{
        //    overrideRoutine = StartCoroutine(OverrideFollowLogic());
        //}

        // Reset if no loot in loot hashSet and in overriden state
        if (!hasLootThreshhold && overridden)
        {
            ResetFollowLogic();
        }

        // If loot-hashSet has no loot, reset override bool and stop Coroutine. 
        if (!hasLootThreshhold && overrideCoroutineRunning)
        {
            StopCoroutine(overrideRoutine);
            overrideCoroutineRunning = false;
        }
    }

    private IEnumerator OverrideFollowLogic()
    {
        overrideCoroutineRunning = true;

        yield return new WaitForSeconds(overrideDelay);

        if (LootManager.instance != null && LootManager.instance.DroppedLoot.Count > 0 && !overridden)
        {
            Event_System.instance?.OnPullAllLoot?.Invoke();
            //Debug.Log("INVOKED OVERRIDE LOGIC");

            overridden = true;
        }

        overrideCoroutineRunning = false;
    }

    private void ResetFollowLogic()
    {
        Event_System.instance?.OnResetPullAllLoot?.Invoke();
        //Debug.Log("INVOKED RESET LOGIC");

        overridden = false;
    }
}