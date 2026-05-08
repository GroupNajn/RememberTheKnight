using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Randomize Float Range", story: "Sets [Value] to a value between [From] and [To]", category: "Action", id: "0c600289d93f38cb6434383736bef843")]
public partial class RandomizeFloatRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Value;
    [SerializeReference] public BlackboardVariable<float> From;
    [SerializeReference] public BlackboardVariable<float> To;

    protected override Status OnStart()
    {
        Value.Value = UnityEngine.Random.Range(From.Value, To.Value);
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

