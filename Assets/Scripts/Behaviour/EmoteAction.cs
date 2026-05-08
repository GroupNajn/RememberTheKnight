using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Execute Emote", story: "Executes [EmoteName] in [Self] and waits until animation is finished", category: "Action", id: "d5e1ed687b441b90bef3f1b5e43995a1")]
public partial class EmoteAction : Action
{
    [SerializeReference] public BlackboardVariable<string> EmoteName;
    [SerializeReference] public BlackboardVariable<Animator> Self;
    [SerializeReference] public BlackboardVariable<bool> IsEmoting = new();
    [SerializeReference] public BlackboardVariable<float> SecondsTimeout = new(5);

    private NavMeshAgent navMeshAgent;
    private float elapsedSeconds = 0f;
    private float lastTime = 0f;
    private bool hasStartedEmote = false;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;
        navMeshAgent = Self.Value.gameObject.GetComponent<NavMeshAgent>();
        lastTime = Time.time;
        Self.Value.SetTrigger(EmoteName.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (elapsedSeconds >= SecondsTimeout.Value) return Status.Failure;
        if (!hasStartedEmote)
        {
            elapsedSeconds += Time.time - lastTime;
            lastTime = Time.time;
            hasStartedEmote = IsEmoting.Value;
        }

        if (hasStartedEmote)
        {
            if (navMeshAgent) navMeshAgent.nextPosition = Self.Value.transform.position;
            if (!IsEmoting.Value) return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        elapsedSeconds = 0f;
        IsEmoting.Value = false;
        hasStartedEmote = false;
        if (navMeshAgent) navMeshAgent.nextPosition = Self.Value.transform.position;
    }
}