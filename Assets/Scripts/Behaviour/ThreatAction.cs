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
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Transform> NavTarget;
    [SerializeReference] public BlackboardVariable<float> Radius;

    [SerializeReference] public BlackboardVariable<float> AttackRadius = new(1);

    protected override Status OnStart()
    {
        if (Target.Value == null || Self.Value == null) return Status.Failure;

        var dist = Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position);

        if (dist > Radius)
        {
            ThreatValue.Value = 0;
        }
        else
        { ThreatValue.Value = MathF.Round(1 - (dist / (Radius.Value + AttackRadius)), 2); /*Debug.Log($"current threat{ThreatValue.Value}");*/ }

        if (NavTarget.Value != null)
        {
            bool isNotInSight =
                !Mathf.Approximately(Target.Value.transform.position.x, Target.Value.transform.position.x) &&
                !Mathf.Approximately(Target.Value.transform.position.z, Target.Value.transform.position.z);
            if (isNotInSight)
            {
                bool isAggro = ThreatValue.Value > 0.4f;
                bool isAlert = ThreatValue.Value > 0.2f;
                ThreatValue.Value = isAggro ? 0.4f : ThreatValue.Value;
                ThreatValue.Value = isAlert ? 0.2f : ThreatValue.Value;
            }
        }
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

