using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Animation State", story: "Sets [bool] If Current [Animator] State In Layer [LayerIndex] Is [Name]", category: "Action/Animation", id: "898a7775ba167e0ff8f049c64af70408")]
public partial class GetAnimationStateAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Bool;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<int> LayerIndex;
    [SerializeReference] public BlackboardVariable<string> Name;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        Bool.Value = Animator.Value.GetCurrentAnimatorStateInfo(LayerIndex.Value).IsName(Name);
        //Debug.Log(Bool.Value ? "Attacking" : "Not attacking");
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

