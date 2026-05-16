using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Select Agents to Engage", story: "Selects up to [Count] of the closest agents to [Target] and moves them between [WaitList] and [AggroList]", category: "Action", id: "c0bba413ffd702563a49fc63381a17d6")]
public partial class SelectAgentsToEngageAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Count;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<List<GameObject>> WaitList;
    [SerializeReference] public BlackboardVariable<List<GameObject>> AggroList;
    [SerializeReference] public BlackboardVariable<LostAggroEvent> DisengageEventChannel;

    private HashSet<GameObject> currentlyEngaged = new();
    protected override Status OnStart()
    {
        if (WaitList.Value == null || AggroList.Value == null || DisengageEventChannel.Value == null)
            return Status.Failure;

        currentlyEngaged.RemoveWhere(a => a == null);

        var allAgents = new List<GameObject>(AggroList.Value);
        allAgents.AddRange(WaitList.Value);
        allAgents = allAgents.Distinct().ToList();
        int allAgentCount = allAgents.Count;
        if (allAgentCount == 0) return Status.Success;


        allAgents.Sort((agentA, agentB) =>
        {
            var distA = Vector3.Distance(agentA.transform.position, Target.Value.transform.position);
            var distB = Vector3.Distance(agentB.transform.position, Target.Value.transform.position);
            return Convert.ToInt32(distA - distB);
        });

        int selectCount = Math.Min(Count.Value, allAgentCount);
        var newSelection = new HashSet<GameObject>();
        for (int i = 0; i < selectCount; i++)
        {
            var selectedAgent = allAgents[i];
            newSelection.Add(selectedAgent);

            if (!AggroList.Value.Contains(selectedAgent))
            {
                AggroList.Value.Add(selectedAgent);
                WaitList.Value.Remove(selectedAgent);
            }
        }

        foreach (var agent in currentlyEngaged)
        {
            if (!newSelection.Contains(agent) && AggroList.Value.Contains(agent))
            {
                DisengageEventChannel.Value.SendEventMessage(agent, Target.Value.gameObject);
                AggroList.Value.Remove(agent);
                WaitList.Value.Add(agent);
            }
        }

        currentlyEngaged = newSelection;

        AggroList.Value = currentlyEngaged.ToList();

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

