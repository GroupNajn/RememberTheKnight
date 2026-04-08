using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Execute Animation", story: "Sets animation trigger [triggerName] in [Self] and waits until animation [stateName] is finished", category: "Action", id: "d5e1ed687b441b90bef3f1b5e43995a1")]
public partial class ExecuteAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<string> TriggerName;
    [SerializeReference] public BlackboardVariable<string> StateName;
    [SerializeReference] public BlackboardVariable<Animator> Self;
    [SerializeReference] public BlackboardVariable<bool> IsWaiting = new(false);

    private bool isWaiting = false;
    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;
        Self.Value.SetTrigger(TriggerName.Value);
        IsWaiting.Value = true;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (!IsWaiting.Value) return Status.Success;
        AnimatorStateInfo stateInfo = Self.Value.GetCurrentAnimatorStateInfo(0);


        if (stateInfo.IsName(StateName.Value) && stateInfo.normalizedTime < 1.0f)
        {
            return Status.Running;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
        IsWaiting.Value = false;
    }
}

