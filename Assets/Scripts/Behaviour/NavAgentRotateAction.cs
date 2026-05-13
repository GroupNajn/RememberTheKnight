using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NavAgent Rotate", story: "Rotates [Self] towards [Target] using angular speed", category: "Action", id: "2c68b0a3922b9749a9edad1917c6af27")]
public partial class NavAgentRotateAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Tolerance = new(10);
    [SerializeReference] public BlackboardVariable<bool> Continuous = new(false);


    protected override Status OnStart()
    {
        if (Target.Value == null) return Status.Success;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 direction = Target.Value.position - Self.Value.transform.position;
        direction.y = 0;

        if (direction == Vector3.zero)
            return Continuous.Value ? Status.Running : Status.Success;

        Quaternion desiredRotation = Quaternion.LookRotation(direction.normalized);
        bool isDone = Quaternion.Angle(Self.Value.transform.rotation, desiredRotation) < Tolerance.Value;

        if (isDone && !Continuous.Value) return Status.Success;

        Self.Value.transform.rotation = Quaternion.RotateTowards(
            Self.Value.transform.rotation,
            desiredRotation,
            Self.Value.angularSpeed * Time.deltaTime
        );
        return Status.Running;
    }

    protected override void OnEnd()
    {

    }
}

