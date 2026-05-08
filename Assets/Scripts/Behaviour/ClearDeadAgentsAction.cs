using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Clear Dead Agents", story: "Clears dead agents in [List]", category: "Action", id: "e243a6d71b1efd986a2614b41b2b75d7")]
public partial class ClearDeadAgentsAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;

    protected override Status OnStart()
    {
        if (List.Value == null) return Status.Failure;
        List.Value.RemoveAll(obj =>
        {
            if (obj.TryGetComponent(out EnemyDamage enemyDamage))
            {
                return enemyDamage.Health <= 0;
            }
            return true;
        });
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

