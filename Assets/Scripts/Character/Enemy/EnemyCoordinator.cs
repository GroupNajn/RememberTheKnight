using System.Collections;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

/// <summary>
/// Provides information about enemies that other componeents can use,
/// Actual coordination is done inside a <see cref="BehaviorGraph"/>
/// </summary>
/// <remarks>
/// <list>
/// <item>See <see cref="SelectAgentsToEngageAction"/></item>
/// <item>See <see cref="SyncAggroAgentsAction"/></item>
/// <item>Author: Theo Johansson</item>
/// </list>
/// </remarks>
[RequireComponent(typeof(BehaviorGraphAgent))]
public class EnemyCoordinator : MonoBehaviour
{
    private static readonly WaitForSeconds checkDelay = new WaitForSeconds(2);
    private BehaviorGraphAgent graphAgent;
    private BlackboardVariable<List<GameObject>> aggroAgents;
    private BlackboardVariable<List<GameObject>> aggroRangedAgents;
    private BlackboardVariable<List<GameObject>> waitingAgents;
    private Coroutine checkCombatCoroutine;
    private int totalEnemies = 0;

    [Header("Combat Status")]
    public bool CombatEncounterInProgress = false;


    void Start()
    {
        graphAgent = GetComponent<BehaviorGraphAgent>();

        BlackboardReference coordinatorData = graphAgent.BlackboardReference;

        if (coordinatorData.GetVariable("Aggro Agents", out aggroAgents)) { }

        if (coordinatorData.GetVariable("Aggro Ranged Agents", out aggroRangedAgents)) { }

        if (coordinatorData.GetVariable("Waiting Agents", out waitingAgents)) { }
        checkCombatCoroutine = StartCoroutine(CheckCombat());

    }

    void OnDestroy()
    {
        StopCoroutine(checkCombatCoroutine);
    }
    IEnumerator CheckCombat()
    {
        while (true)
        {

            if (aggroAgents.Value != null && aggroRangedAgents.Value != null && waitingAgents.Value != null)
            {
                totalEnemies = aggroAgents.Value.Count + aggroRangedAgents.Value.Count + waitingAgents.Value.Count;
                CombatEncounterInProgress = totalEnemies > 0;
            }
            else
            {
                totalEnemies = 0;
                CombatEncounterInProgress = false;
            }
            yield return checkDelay;
        }
    }

}
