using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set NavMeshAgent Priority", story: "Sets the avoidance priority of [Self] to [Value]", category: "Action", id: "b193cce9bab439d27bc3a042a1bab5ea")]
public partial class SetNavMeshAgentPriorityAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;
        Self.Value.avoidancePriority = Value.Value;
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

