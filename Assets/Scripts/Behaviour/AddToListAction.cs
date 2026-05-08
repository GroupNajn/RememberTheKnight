using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Add To List", story: "Add [Object] to [List]", category: "Action", id: "63dd8543bf7910804f67e20efe257b33")]
public partial class AddToListAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;

    protected override Status OnStart()
    {
        if (Object.Value == null || List.Value == null)
            return Status.Failure;

        if (!List.Value.Contains(Object.Value))
            List.Value.Add(Object.Value);

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

