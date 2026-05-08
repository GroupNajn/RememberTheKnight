using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Increment Integer", story: "Increments [Value] by [Step]", category: "Action", id: "fc28ea9dcf0e0121de49fe818b79c196")]
public partial class IncrementIntegerAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Value;
    [SerializeReference] public BlackboardVariable<int> Step = new(1);

    protected override Status OnStart()
    {
        Value.Value += Step.Value;
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

