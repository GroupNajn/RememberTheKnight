using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

/// <summary>
/// Teleports a target to a target location and plays particles to show the teleportation
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Teleport To", story: "Teleports [Self] to [Transform]", category: "Action", id: "b94cfaa7f0b39bbffb23e0d05ef160fe")]
public partial class TeleportToAction : Action
{
    private static readonly int CancelHash = Animator.StringToHash("Cancel");
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Transform;
    [SerializeReference] public BlackboardVariable<List<GameObject>> ToDisable;
    [SerializeReference] public BlackboardVariable<ParticleSystem> LeaveEffect;
    [SerializeReference] public BlackboardVariable<ParticleSystem> EnterEffect;
    [SerializeReference] public BlackboardVariable<float> SecondsTimer = new(1.5f);

    ParticleSystem leave;
    ParticleSystem enter;

    CharacterController characterController;
    Animator animator;
    bool isEntering;
    float timeRemaing;
    protected override Status OnStart()
    {
        characterController = Self.Value.GetComponent<CharacterController>();
        animator = Self.Value.GetComponent<Animator>();
        animator.SetTrigger(CancelHash);

        // when teleporting the character controller is disabled due to it does not like being translated large distances 
        characterController.enabled = false;
        leave = UnityEngine.Object.Instantiate(LeaveEffect.Value, Self.Value.transform.position, Quaternion.identity);
        leave.Play();
        Self.Value.transform.position = Transform.Value.position;
        ToDisable.Value.ForEach(obj => obj.SetActive(false));
        timeRemaing = SecondsTimer.Value;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (leave.isEmitting) return Status.Running;
        if (!enter && !isEntering)
        {
            enter = UnityEngine.Object.Instantiate(EnterEffect.Value, Self.Value.transform.position, Quaternion.identity);
            enter.Play();
            isEntering = true;
        }
        if (timeRemaing > 0) timeRemaing -= Time.deltaTime;
        if (timeRemaing <= 0) ToDisable.Value.ForEach(obj => obj.SetActive(true));
        if (!enter.isEmitting) return Status.Success;
        return Status.Running;
    }

    protected override void OnEnd()
    {
        isEntering = false;
        characterController.enabled = true;
        ToDisable.Value.ForEach(obj => obj.SetActive(true));
        animator.SetTrigger(CancelHash);
        UnityEngine.Object.Destroy(leave);
        UnityEngine.Object.Destroy(enter);
    }
}

