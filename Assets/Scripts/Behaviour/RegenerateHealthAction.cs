using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

/// <summary>
/// Regenerates Health up to a thresholdd of an agent either continuosly or until that threshold is reached
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Regenerate Health", story: "[Self] regenerates [Amount] health per second", category: "Action", id: "d9518b0bb28bebf09118faa271add6eb")]
public partial class RegenerateHealthAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyDamage> Self;
    [SerializeReference] public BlackboardVariable<float> Amount;

    [SerializeReference] public BlackboardVariable<float> NormalizedTarget = new(1f);
    [SerializeReference] public BlackboardVariable<bool> Infinite = new(true);


    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value.Health < Self.Value.MaxHealth * NormalizedTarget.Value)
        {
            Self.Value.Health += Amount.Value * Time.deltaTime;
            return Status.Running;
        }
        else return Infinite.Value ? Status.Running : Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

