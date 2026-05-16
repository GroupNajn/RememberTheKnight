using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check if child", story: "Checks if [TargetObject] is child of [ParentObject]", category: "Action/Conditional", id: "6b99b112df1e5a0125ce73a671a8ecc1")]
public partial class CheckIfChildAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> TargetObject;
    [SerializeReference] public BlackboardVariable<GameObject> ParentObject;

    protected override Status OnStart()
    {
        if (TargetObject.Value == null || ParentObject.Value == null) return Status.Failure;
        return TargetObject.Value.transform.IsChildOf(ParentObject.Value.transform) ? Status.Success : Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

