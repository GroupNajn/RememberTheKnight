using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

/// <summary>
/// Removes a gameObject from a list inside a <see cref="BehaviorGraph"/>
/// </summary>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Remove From List", story: "Removes [Object] from [List]", category: "Action", id: "a784068e9efc9807fa9d070cef50d73c")]
public partial class RemoveFromListAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;

    protected override Status OnStart()
    {
        if (Object.Value == null || List.Value == null)
            return Status.Failure;

        List.Value.Remove(Object.Value);
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

