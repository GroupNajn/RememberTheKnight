using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

/// <summary>
/// Plays a random particle system that exists in the scene every couple seconds
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Play Random Particle System", story: "Plays a random placed particle system from [ParticleList] every [Interval] seconds", category: "Action", id: "4ffacfa404ab31833ca6e21ddbe57187")]
public partial class PlayRandomParticleSystemAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> ParticleList;
    [SerializeReference] public BlackboardVariable<float> Interval;
    float cooldown = 0;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0) return Status.Running;
        if (ParticleList.Value[UnityEngine.Random.Range(0, ParticleList.Value.Count)].TryGetComponent<ParticleSystem>(out var particleSystem))
        {
            if (!particleSystem.isEmitting)
            {
                particleSystem.Play();
                cooldown = Interval.Value;
            }
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

