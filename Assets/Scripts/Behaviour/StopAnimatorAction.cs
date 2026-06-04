using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

/// <summary>
/// Stops the movmeent of an <see cref="Animator"/> using the <see cref="NavMeshAgent"/>s acceleration scaled by a BreakingMultiplier
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Stop NavMesh Agent", story: "Stops the movment of [Self]", category: "Action", id: "dc10c2beb3ef1a3683753e97e049d950")]
public partial class StopAnimatorAction : Action
{
    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<float> BreakingMultiplier = new(3);

    private Animator animator;
    protected override Status OnStart()
    {
        animator = Self.Value.gameObject.GetComponent<Animator>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float currentSpeedX = animator.GetFloat(XHash);
        float currentSpeedZ = animator.GetFloat(YHash);

        animator.SetFloat(XHash, Mathf.Lerp(currentSpeedX, 0, BreakingMultiplier.Value * Self.Value.acceleration * Time.deltaTime));
        animator.SetFloat(YHash, Mathf.Lerp(currentSpeedZ, 0, BreakingMultiplier.Value * Self.Value.acceleration * Time.deltaTime));

        currentSpeedX = animator.GetFloat(XHash);
        currentSpeedZ = animator.GetFloat(YHash);
        if (currentSpeedX < 0.01f && currentSpeedZ < 0.01f) return Status.Success;
        return Status.Running;
    }

    protected override void OnEnd()
    {

        animator.SetFloat(XHash, 0);
        animator.SetFloat(YHash, 0);
    }
}

