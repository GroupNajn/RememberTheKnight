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
    [SerializeReference] public BlackboardVariable<bool> Continuous = new(false);
    [SerializeReference] public BlackboardVariable<float> SpeedMultiplier = new(1);


    protected override Status OnStart()
    {
        if (Target.Value == null) return Status.Success;
        Self.Value.updateRotation = false;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 direction = Target.Value.position - Self.Value.transform.position;
        direction.y = 0;

        if (direction == Vector3.zero || direction.magnitude < 0.001f)
            return Continuous.Value ? Status.Running : Status.Success;
        else
        {

            Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
            Self.Value.transform.rotation = Quaternion.RotateTowards(
                Self.Value.transform.rotation,
                desiredRotation,
                Mathf.Min(
                    Self.Value.angularSpeed * Time.deltaTime * SpeedMultiplier.Value,
                    angle
                )
            );
            if (Continuous.Value) return Status.Running;

            var currentRotation = Self.Value.transform.rotation.eulerAngles;
            var desiredRotationEuler = desiredRotation.eulerAngles;
            bool isDone =
                Mathf.Approximately(currentRotation.x, desiredRotationEuler.x) &&
                Mathf.Approximately(currentRotation.y, desiredRotationEuler.y) &&
                Mathf.Approximately(currentRotation.z, desiredRotationEuler.z);

            if (isDone) return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Self.Value.updateRotation = true;
    }
}

