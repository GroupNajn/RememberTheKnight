using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[BlackboardEnum]
public enum EqualityOperator
{
    EqualTo,
    NotEqualTo,
    GreaterThan,
    LowerThan,
    GreaterOrEqualTo,
    LowerOrEqualTo
}

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ThreatCancelSignal", story: "Sets [Cancel] Signal if [Threat] is [Equality] [Value]", category: "Action", id: "3e505226a506d73603e7f7bb082ec80d")]
public partial class ThreatCancelSignalAction : Action
{

    [SerializeReference] public BlackboardVariable<bool> Cancel;
    [SerializeReference] public BlackboardVariable<float> Threat;
    [SerializeReference] public BlackboardVariable<EqualityOperator> Equality = new(EqualityOperator.LowerThan);
    [SerializeReference] public BlackboardVariable<float> Value;


    protected override Status OnStart()
    {
        switch (Equality.Value)
        {
            case EqualityOperator.EqualTo:
                Cancel.Value = Threat.Value == Value.Value;
                return Status.Success;
            case EqualityOperator.NotEqualTo:
                Cancel.Value = Threat.Value != Value.Value;
                break;
            case EqualityOperator.GreaterThan:
                Cancel.Value = Threat.Value > Value.Value;
                return Status.Success;
            case EqualityOperator.GreaterOrEqualTo:
                Cancel.Value = Threat.Value >= Value.Value;
                return Status.Success;
            case EqualityOperator.LowerThan:
                Cancel.Value = Threat.Value < Value.Value;
                return Status.Success;
            case EqualityOperator.LowerOrEqualTo:
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

