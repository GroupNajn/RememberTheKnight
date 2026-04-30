using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Execute Attack", story: "Executes [AttackName] in [Self] and waits until attack is finished", category: "Action", id: "ef1e95d1ab6c3ff8e5dc52964d0d5daf")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<string> AttackName;
    [SerializeReference] public BlackboardVariable<Animator> Self;
    [SerializeReference] public BlackboardVariable<bool> IsAttacking = new();
    [SerializeReference] public BlackboardVariable<float> SecondsTimeout = new(20);

    private float elapsedSeconds = 0f;
    private bool hasStartedAttack = false;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;

        Self.Value.SetTrigger(AttackName.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (elapsedSeconds >= SecondsTimeout.Value) return Status.Failure;
        elapsedSeconds += Time.deltaTime;
        if (IsAttacking.Value && !hasStartedAttack) { hasStartedAttack = true; }
        if (!IsAttacking.Value && hasStartedAttack)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        elapsedSeconds = 0f;
        IsAttacking.Value = false;
        hasStartedAttack = false;
    }
}

