using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NavAgent Rotate Angle", story: "Rotates [Self] by [Angle] ° using angular speed", category: "Action", id: "86ea72e17ccda1be73ea51613314dc6a")]
public partial class NavAgentRotateAngleAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<float> Angle;
    private float remainingDegrees;
    protected override Status OnStart()
    {
        remainingDegrees = Mathf.Abs(Angle.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (remainingDegrees <= 0) return Status.Success;
        Self.Value.transform.Rotate(Vector3.up, Angle.Value * Time.deltaTime);
        remainingDegrees -= Mathf.Abs(Angle.Value * Time.deltaTime);
        return Status.Running;
    }

    protected override void OnEnd()
    {

    }
}

