using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

/// <summary>
/// Changes the <see cref="Phase"/> enum when the boss agent reaches a health threshold
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Boss Phase", story: "Sets the current [CurrentPhase] based on health in [Self]", category: "Action", id: "abd0b063ed2e18c1d2042d22fa86d0e1")]
public partial class SetBossPhaseAction : Action
{
    [SerializeReference] public BlackboardVariable<Phase> CurrentPhase;
    [SerializeReference] public BlackboardVariable<EnemyDamage> Self;
    [SerializeReference] public BlackboardVariable<float> PhaseShiftThreshold = new(0.5f);

    protected override Status OnStart()
    {
        float healthNormalized = Self.Value.Health / Self.Value.MaxHealth;
        if (healthNormalized <= PhaseShiftThreshold && CurrentPhase.Value == Phase.Initial) CurrentPhase.Value = Phase.Intermission;
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

