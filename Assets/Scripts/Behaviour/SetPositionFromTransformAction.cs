using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

/// <summary>
/// Saves the position of a transform inside a vector3 blackboard variable
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Position From Transform", story: "Sets [Position] to [Transform] position", category: "Action", id: "c6ea43dadf1e38ba40957759251f840d")]
public partial class SetPositionFromTransformAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Position;
    [SerializeReference] public BlackboardVariable<Transform> Transform;

    protected override Status OnStart()
    {
        if (Position.Value == null || Transform.Value == null) return Status.Failure;
        Position.Value = Transform.Value.position;
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

