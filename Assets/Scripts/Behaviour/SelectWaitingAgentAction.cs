using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEditor.Rendering;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Select Waiting Agent", story: "Select [Agent] from [Waitlist] when [Aggrolist] has fewer than [Count] entries", category: "Action", id: "acfa597d9d3f5a396bb4920ebd434ee5")]
public partial class SelectWaitingAgentAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waitlist;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Aggrolist;
    [SerializeReference] public BlackboardVariable<int> Count;

    protected override Status OnStart()
    {
        if (Waitlist.Value == null || Aggrolist.Value == null) return Status.Failure;
        if (Waitlist.Value.Count == 0) return Status.Running;
        if (Aggrolist.Value.Count >= Count.Value) return Status.Running;
        Agent.Value = Waitlist.Value[0];
        Waitlist.Value.RemoveAt(0);
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (Waitlist.Value.Count == 0) return Status.Running;
        if (Aggrolist.Value.Count >= Count.Value) return Status.Running;
        Agent.Value = Waitlist.Value[0];
        Waitlist.Value.RemoveAt(0);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

