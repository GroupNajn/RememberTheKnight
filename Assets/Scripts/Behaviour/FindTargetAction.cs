using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Target", story: "Find the nearest [Target] with tag [tagname]", category: "Action", id: "ac8eddcf9c7e6fad5e0d218a5dbaa1bc")]
public partial class FindTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<string> Tagname;

    protected override Status OnStart()
    {
        if (Target.Value != null) { return Status.Success; }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tagname.Value);
        if (gameObjects.Length > 0)
        {
            if (gameObjects.Length == 1)
            {
                Target.Value = gameObjects[0];
                return Status.Success;
            }
            int shortestIndex = 0;
            float shortestDistance = float.MaxValue;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                float distance = Vector3.Distance(Self.Value.transform.position, gameObjects[i].transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    shortestIndex = i;
                }
            }
            Target.Value = gameObjects[shortestIndex];
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

