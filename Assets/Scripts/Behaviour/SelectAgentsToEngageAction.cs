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

    protected override Status OnStart()
    {
        if (WaitList.Value == null || AggroList.Value == null || DisengageEventChannel.Value == null)
            return Status.Failure;

        List<GameObject> allAgents;

        if (AggroList.Value.Count > 0)
            allAgents = new(AggroList.Value);
        else
            allAgents = new();

        if (WaitList.Value.Count > 0)
            allAgents.AddRange(WaitList.Value);

        int allAgentCount = allAgents.Count;
        if (allAgentCount == 0) return Status.Success;
        allAgents.Sort((agentA, agentB) =>
        {
            var distA = Vector3.Distance(agentA.transform.position, Target.Value.transform.position);
            var distB = Vector3.Distance(agentB.transform.position, Target.Value.transform.position);
            return Convert.ToInt32(distA - distB);
        });

        GameObject[] selectedAgents = new GameObject[allAgentCount > Count.Value ? Count.Value : allAgentCount];
        for (int i = 0; i < selectedAgents.Length; i++)
        {
            var selectedAgent = allAgents[i];
            selectedAgents[i] = selectedAgent;
            if (!AggroList.Value.Contains(selectedAgent)) AggroList.Value.Add(selectedAgent);
        }

        List<GameObject> snapshot = new(AggroList.Value);
        foreach (var agent in snapshot)
        {
            if (!selectedAgents.Contains(agent))
            {
                DisengageEventChannel.Value.SendEventMessage(agent, Target.Value.gameObject);
            }
        }

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

