using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

/// <summary>
/// A random integer in a range that can be used inside a <see cref="BehaviorGraph"/>
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Randomize Integer Range", story: "Sets [Value] to a value betweeon [From] and [To]", category: "Action", id: "83348eb66835d94e6ce6c0e0bea16e4a")]
public partial class RandomizeIntegerRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Value;
    [SerializeReference] public BlackboardVariable<int> From;
    [SerializeReference] public BlackboardVariable<int> To;

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

