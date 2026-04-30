using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LineOfSight", story: "Sets [NavigationTarget] when [Target] is in sight", category: "Action", id: "462f441212c9758ad05609aae132293a")]
public partial class LineOfSightAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> NavigationTarget;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> ViewAngle = new(45);
    GameObject navigationTarget;
    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;
        if (NavigationTarget.Value == null)
        {
            navigationTarget = new("Navigation Target");
            navigationTarget.transform.parent = Self.Value.transform;
            navigationTarget.transform.localPosition = Vector3.zero;
            NavigationTarget.Value = navigationTarget.transform;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {


        var forward = Self.Value.transform.forward;
        var dir = (Target.Value.transform.position - Self.Value.transform.position).normalized;
        float angle = Mathf.Rad2Deg * Mathf.Acos(
            Vector2.Dot(
                new Vector2(forward.x, forward.z),
                new Vector2(dir.x, dir.z)
            )
        );
        if (Mathf.Abs(angle) <= ViewAngle.Value)
            NavigationTarget.Value.position = Target.Value.transform.position;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

