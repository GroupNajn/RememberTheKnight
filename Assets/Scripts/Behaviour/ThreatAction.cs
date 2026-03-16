using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Threat", story: "Sets [threatValue] based on distance between [Self] and [Target] when inside [radius]", category: "Action", id: "d30db1d6c9bd7521a4a25bb7bc54a70a")]
public partial class ThreatAction : Action
{
    [SerializeReference] public BlackboardVariable<float> ThreatValue;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Radius;

    [SerializeReference] public BlackboardVariable<float> AttackRadius = new(1);
    protected override Status OnStart()
    {
        if (ThreatValue == null) return Status.Success;
        var dist = Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position);

        if (dist > Radius)
        {
            ThreatValue.Value = 0;
        }
        else
        { ThreatValue.Value = MathF.Round(1 - (dist / (Radius.Value + AttackRadius)), 2); Debug.Log($"current threat{ThreatValue.Value}"); }
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

