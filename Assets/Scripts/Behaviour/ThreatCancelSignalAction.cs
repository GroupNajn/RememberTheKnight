using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ThreatCancelSignal", story: "Sets [Cancel] Signal if [Threat] is [Equality] than [Value]", category: "Action", id: "3e505226a506d73603e7f7bb082ec80d")]
public partial class ThreatCancelSignalAction : Action
{

    [SerializeReference] public BlackboardVariable<bool> Cancel;
    [SerializeReference] public BlackboardVariable<float> Threat;
    [SerializeReference] public BlackboardVariable<ConditionOperator> Equality;
    [SerializeReference] public BlackboardVariable<float> Value;


    protected override Status OnStart()
    {
        switch (Equality.Value)
        {
            case ConditionOperator.Equal:
                Cancel.Value = Threat.Value == Value.Value;
                return Status.Success;
            case ConditionOperator.Greater:
                Cancel.Value = Threat.Value > Value.Value;
                return Status.Success;
            case ConditionOperator.GreaterOrEqual:
                Cancel.Value = Threat.Value >= Value.Value;
                return Status.Success;
            case ConditionOperator.Lower:
                Cancel.Value = Threat.Value < Value.Value;
                return Status.Success;
            case ConditionOperator.LowerOrEqual:
                Cancel.Value = Threat.Value <= Value.Value;
                return Status.Success;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

