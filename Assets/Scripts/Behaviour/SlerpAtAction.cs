using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Slerp At", story: "[Transform] slerps at [Target]", category: "Action", id: "d426bda9383a51106e8909c618f3a33f")]
public partial class SlerpAtAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Transform;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    [SerializeReference] public BlackboardVariable<bool> Continuous = new(false);
    [SerializeReference] public BlackboardVariable<bool> LimitToYAxis = new(false);
    [SerializeReference] public BlackboardVariable<float> SlerpSpeed = new(2f);

    protected override Status OnStart()
    {
        if (Transform.Value == null || Target.Value == null)
        {
            LogFailure($"Missing Transform or Target.");
            return Status.Failure;
        }

        ProcessSlerpAt();
        return Continuous.Value ? Status.Running : Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (Continuous.Value)
        {
            ProcessSlerpAt();
            return Status.Running;
        }
        return Status.Success;
    }

    void ProcessSlerpAt()
    {
        Vector3 direction = Target.Value.position - Transform.Value.position;
        if (LimitToYAxis.Value)
        {
            direction.y = 0;
        }

        direction = direction.normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Transform.Value.rotation = Quaternion.Slerp(Transform.Value.rotation, targetRotation, SlerpSpeed.Value * Time.deltaTime);
        }
    }
}

