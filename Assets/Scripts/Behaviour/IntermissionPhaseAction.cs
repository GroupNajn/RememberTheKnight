using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Intermission Phase", story: "Spawns [Count] adds using [Spawner] succeeds when Spawner is complete", category: "Action", id: "7fe30b4982614665999dc3c5fdadbe0b")]
public partial class IntermissionPhaseAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Count;
    [SerializeReference] public BlackboardVariable<BossAddSpawnerManager> Spawner;

    protected override Status OnStart()
    {
        Spawner.Value.SetSpawnTarget(Count.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Spawner.Value.KilledAdds >= Count.Value) return Status.Success;
        Spawner.Value.SpawnEnemies();
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

