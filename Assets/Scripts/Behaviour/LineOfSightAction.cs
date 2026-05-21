using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;
using System.Collections.Generic;

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
    [SerializeReference] public BlackboardVariable<float> YOffset = new(1f);
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
        foreach (var hit in Physics.RaycastAll(Self.Value.position + new Vector3(0, YOffset.Value, 0), dir, MaxDistance.Value, excludeEnemies))
        {
            if (!hit.collider.transform.IsChildOf(Self.Value))
            {
                if (hit.collider.CompareTag(TargetTag.Value))
                {
                    HasSight.Value = true;
                    TargetPosition.Value = Target.Value.position;
                }
                break;
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

