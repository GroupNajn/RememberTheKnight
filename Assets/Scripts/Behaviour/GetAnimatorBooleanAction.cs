using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Animator Boolean", story: "Gets [Name] from [Self] and saves it in [Value]", category: "Action", id: "f1b3c9452e7542549619cc369b65ee1c")]
public partial class GetAnimatorBooleanAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Value;
    [SerializeReference] public BlackboardVariable<Animator> Self;
    [SerializeReference] public BlackboardVariable<string> Name;
    public BlackboardVariable<bool> Continuous = new();

    private int nameHash;
    protected override Status OnStart()
    {
        if (Name.Value == null || Self.Value == null) return Status.Failure;
        nameHash = Animator.StringToHash(Name.Value);

        Value.Value = Self.Value.GetBool(nameHash);

        if (Continuous.Value)
            return Status.Running;

        return Status.Success;
    }

    protected override Status OnUpdate()
    {

        Value.Value = Self.Value.GetBool(nameHash);
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

