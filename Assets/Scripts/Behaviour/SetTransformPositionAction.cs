using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

/// <summary>
/// Sets a transforms position inside a <see cref="BehaviorGraph"/>
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetTransformPosition", story: "Set [Postion] in [Transform]", category: "Action", id: "fd6d9cd035fef401c5b9b030491ba22c")]
public partial class SetTransformPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Postion;
    [SerializeReference] public BlackboardVariable<Transform> Transform;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    GameObject transform;
    protected override Status OnStart()
    {

        if (Transform.Value == null)
        {
            transform = new($"{Self.Value.name} Target");
            transform.transform.position = Self.Value.transform.position;
            Transform.Value = transform.transform;

        }
        if (Postion.Value == null) return Status.Failure;
        Transform.Value.position = Postion.Value;
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

