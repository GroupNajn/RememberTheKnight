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
    private static readonly int IsEmotingHash = Animator.StringToHash("IsEmoting");
    [SerializeReference] public BlackboardVariable<string> EmoteName;
    [SerializeReference] public BlackboardVariable<Animator> Self;
    [SerializeReference] public BlackboardVariable<float> SecondsTimeout = new(5);

    private float elapsedSeconds = 0f;
    private float lastTime = 0f;
    private bool hasStartedEmote = false;
    private bool isEmoting = false;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;
        lastTime = Time.time;
        Self.Value.SetTrigger(EmoteName.Value);
        isEmoting = Self.Value.GetBool(IsEmotingHash);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (elapsedSeconds >= SecondsTimeout.Value) return Status.Failure;
        isEmoting = Self.Value.GetBool(IsEmotingHash);

        if (isEmoting && !hasStartedEmote)
            hasStartedEmote = true;

        if (!hasStartedEmote)
        {
            elapsedSeconds += Time.time - lastTime;
            lastTime = Time.time;
            hasStartedEmote = isEmoting;
        }

        if (hasStartedEmote && !isEmoting)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        elapsedSeconds = 0f;
        hasStartedEmote = false;
    }
}