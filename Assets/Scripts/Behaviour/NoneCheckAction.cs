using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "None Check", story: "Sets [Boolean] if [Target] is not None", category: "Action", id: "55fc462d24169677c0b752c2f751a214")]
public partial class NoneCheckAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Boolean;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        Boolean.Value = Target.Value != null;
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

