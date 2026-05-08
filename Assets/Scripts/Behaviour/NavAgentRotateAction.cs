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
    [SerializeReference] public BlackboardVariable<float> Acceleration = new(0);
    [SerializeReference] public BlackboardVariable<float> Tolerance = new(10);
    [SerializeReference] public BlackboardVariable<bool> Continous = new(false);
    [SerializeReference] public BlackboardVariable<bool> PauseSignal = new(false);


    private float finalAngularSpeed;
    protected override Status OnStart()
    {
        finalAngularSpeed = Self.Value.angularSpeed;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (PauseSignal.Value) return Status.Running;
        Vector3 dir = (Target.Value.position - Self.Value.transform.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(dir);

        bool isDone = Quaternion.Angle(Self.Value.transform.rotation, desiredRotation) < Tolerance.Value;

        if (isDone && !Continous.Value) return Status.Success;

        Self.Value.transform.rotation = Quaternion.RotateTowards(
            Self.Value.transform.rotation,
            desiredRotation,
            finalAngularSpeed * Time.deltaTime
        );
        finalAngularSpeed += finalAngularSpeed * Acceleration.Value * Time.deltaTime;
        return Status.Running;
    }

    protected override void OnEnd()
    {

    }
}

