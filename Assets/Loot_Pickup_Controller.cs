using System.Collections;
using UnityEngine;

public class LootFollowController : MonoBehaviour
{
    [SerializeField] private float overrideDelay = 10f;

    private bool overridden = false;
    private bool overrideCoroutineRunning = false;
    private Coroutine overrideRoutine;

    private void Update()
    {
        if (LootManager.instance == null)
            return;

        bool hasLoot = LootManager.instance.DroppedLoot.Count > 0;

        // Start overrite and not coroutineRunning
        if (hasLoot && !overridden && !overrideCoroutineRunning)
        {
            overrideRoutine = StartCoroutine(OverrideFollowLogic());
        }

        // Reset if not lot and in overriden state
        if (!hasLoot && overridden)
        {
            ResetFollowLogic();
        }

        // If loot gets destroyed before finished reset
        if (!hasLoot && overrideCoroutineRunning)
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
            Debug.Log("INVOKED OVERRIDE LOGIC");

            overridden = true;
        }

        overrideCoroutineRunning = false;
    }

    private void ResetFollowLogic()
    {
        Event_System.instance?.OnResetPullAllLoot?.Invoke();
        Debug.Log("INVOKED RESET LOGIC");

        overridden = false;
    }
}