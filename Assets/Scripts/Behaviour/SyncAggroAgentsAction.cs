using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

/// <summary>
/// This is the core of enemy coordination, they are placed in staggered radii around the target based on distance, the closest enemy gets the closest radius.
/// Using <see cref="GiveAggroEvent"/> to communicate with the <see cref="BehaviorGraphAgent"/>s
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Sync Aggro Agents", story: "Aggro agents in [AggroList] are placed in atleast [Radius] around [Target]", category: "Action", id: "58dcd1cc126a5988c2d99fb9fe92bce7")]
public partial class SyncAggroAgentsAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> AggroList;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Radius;
    [SerializeReference] public BlackboardVariable<GiveAggroEvent> EngageEventChannel;

    protected override Status OnStart()
    {
        if (AggroList.Value == null || Target.Value == null || EngageEventChannel.Value == null)
            return Status.Failure;

        AggroList.Value.Sort((agentA, agentB) =>
        {
            var distA = Vector3.Distance(agentA.transform.position, Target.Value.transform.position);
            var distB = Vector3.Distance(agentB.transform.position, Target.Value.transform.position);
            return Convert.ToInt32(distA - distB);
        });

        for (int i = 0; i < AggroList.Value.Count; i++)
        {
            var agent = AggroList.Value[i];
            if (!agent.TryGetComponent<NavMeshAgent>(out var navMeshAgent)) continue;
            EngageEventChannel.Value.SendEventMessage(agent, Radius.Value + i * navMeshAgent.radius, Target.Value);
        }

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

