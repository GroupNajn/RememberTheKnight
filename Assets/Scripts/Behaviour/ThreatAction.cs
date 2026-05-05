using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Threat", story: "Sets [threatValue] based on distance between [Self] and [Target] when inside [radius]", category: "Action", id: "d30db1d6c9bd7521a4a25bb7bc54a70a")]
public partial class ThreatAction : Action
{
    [SerializeReference] public BlackboardVariable<float> ThreatValue = new(0);
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Radius;

    [SerializeReference] public BlackboardVariable<float> AttackRadius = new(1);
    [SerializeReference] public BlackboardVariable<float> DecaySeconds = new(1);

    protected override Status OnStart()
    {
        if (Target.Value == null || Self.Value == null) return Status.Failure;

        var dist = Vector3.Distance(Self.Value.transform.position, Target.Value.position);
        dist -= AttackRadius.Value;

        float newThreat = dist <= Radius
            ? Mathf.Min(1, MathF.Round(1 - (dist / Radius.Value), 2))
            : 0;


        ThreatValue.Value = newThreat >= ThreatValue.Value
            ? newThreat
            : Mathf.Lerp(ThreatValue.Value, newThreat, Time.deltaTime / DecaySeconds.Value);

        ThreatValue.Value = MathF.Round(ThreatValue.Value, 2);
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

