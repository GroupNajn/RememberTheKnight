using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Execute Attack", story: "Executes [AttackName] in [Self] and waits until attack is finished", category: "Action", id: "ef1e95d1ab6c3ff8e5dc52964d0d5daf")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<string> AttackName;
    [SerializeReference] public BlackboardVariable<Animator> Self;
    [SerializeReference] public BlackboardVariable<bool> IsAttacking = new();
    [SerializeReference] public BlackboardVariable<float> SecondsTimeout = new(5);

    private NavMeshAgent navMeshAgent;
    private float elapsedSeconds = 0f;
    private float lastTime = 0f;
    private bool hasStartedAttack = false;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;
        navMeshAgent = Self.Value.gameObject.GetComponent<NavMeshAgent>();
        lastTime = Time.time;
        Self.Value.SetTrigger(AttackName.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (elapsedSeconds >= SecondsTimeout.Value) return Status.Failure;
        if (!hasStartedAttack)
        {
            elapsedSeconds += Time.time - lastTime;
            lastTime = Time.time;
            hasStartedAttack = IsAttacking.Value;
        }

        if (hasStartedAttack)
        {
            if (navMeshAgent) navMeshAgent.nextPosition = Self.Value.transform.position;
            if (!IsAttacking.Value) return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        elapsedSeconds = 0f;
        IsAttacking.Value = false;
        hasStartedAttack = false;
        if (navMeshAgent) navMeshAgent.nextPosition = Self.Value.transform.position;
    }
}

