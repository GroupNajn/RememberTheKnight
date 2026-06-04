using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

/// <summary>
/// Sets a trigger to execute an attack action in an <see cref="Animator"/> and completes when "IsAttacking" bool is set to false in the <see cref="Animator"/>
/// </summary>
/// <remarks>
/// <list>
/// <item>See <see cref="EnemyLocomotion"/></item>
/// <item>Author: Theo Johansson</item>
/// </list>
/// </remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Execute Attack", story: "Executes [AttackName] in [Self] and waits until attack is finished", category: "Action", id: "ef1e95d1ab6c3ff8e5dc52964d0d5daf")]
public partial class AttackAction : Action
{
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    [SerializeReference] public BlackboardVariable<string> AttackName;
    [SerializeReference] public BlackboardVariable<Animator> Self;
    [SerializeReference] public BlackboardVariable<float> SecondsTimeout = new(5);
    private float elapsedSeconds = 0f;
    private float lastTime = 0f;
    private bool hasStartedAttack = false;
    private bool isAttacking = false;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;
        lastTime = Time.time;
        Self.Value.SetTrigger(AttackName.Value);
        isAttacking = Self.Value.GetBool(IsAttackingHash);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (elapsedSeconds >= SecondsTimeout.Value) return Status.Success;
        isAttacking = Self.Value.GetBool(IsAttackingHash);

        if (isAttacking && !hasStartedAttack)
            hasStartedAttack = true;

        if (!hasStartedAttack)
        {
            elapsedSeconds += Time.time - lastTime;
            lastTime = Time.time;
            hasStartedAttack = isAttacking;
        }

        if (hasStartedAttack && !isAttacking)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        elapsedSeconds = 0f;
        hasStartedAttack = false;
    }
}

