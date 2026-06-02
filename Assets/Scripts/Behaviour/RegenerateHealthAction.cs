using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Regenerate Health", story: "[Self] regenerates [Amount] health per second", category: "Action", id: "d9518b0bb28bebf09118faa271add6eb")]
public partial class RegenerateHealthAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyDamage> Self;
    [SerializeReference] public BlackboardVariable<float> Amount;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value.Health < Self.Value.MaxHealth) Self.Value.Health += Amount.Value * Time.deltaTime;
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

