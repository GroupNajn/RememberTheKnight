using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Scale Float By", story: "Scale [Value] by [Scale]", category: "Action", id: "53acd811610e4a90bd50a1353cc598a4")]
public partial class ScaleFloatByAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Value;
    [SerializeReference] public BlackboardVariable<float> Scale;

    protected override Status OnStart()
    {
        Value.Value *= Scale.Value;
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

