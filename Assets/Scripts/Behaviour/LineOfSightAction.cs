using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Line of Sight", story: "Sets [HasSight] and [TargetPosition] when [Target] is in sight", category: "Action", id: "462f441212c9758ad05609aae132293a")]
public partial class LineOfSightAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> HasSight;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Transform> Self;
    [SerializeReference] public BlackboardVariable<float> ViewAngle = new(45);
    [SerializeReference] public BlackboardVariable<float> MaxDistance = new(100f);
    [SerializeReference] public BlackboardVariable<string> TargetTag = new("Player");
    [SerializeReference] public BlackboardVariable<int> ExcludeLayer;

    protected override Status OnStart()
    {
        if (Self.Value == null || Target.Value == null) return Status.Failure;


        var forward = Self.Value.forward;
        var dir = (Target.Value.position - Self.Value.position).normalized;

        float angle = Vector3.Dot(forward, dir);

        if (angle < Mathf.Cos(Mathf.Deg2Rad * ViewAngle.Value / 2))
        {
            HasSight.Value = false;
            return Status.Success;
        }

        LayerMask excludeEnemies = ~(1 << ExcludeLayer.Value);
        if (Physics.Raycast(Self.Value.position, dir, out RaycastHit hit, MaxDistance.Value, excludeEnemies))
        {
            if (hit.collider.CompareTag(TargetTag.Value))
            {
                HasSight.Value = true;
                TargetPosition.Value = Target.Value.position;
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

