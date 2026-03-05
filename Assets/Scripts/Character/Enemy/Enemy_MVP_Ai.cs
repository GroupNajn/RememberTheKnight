
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_MVP_Ai : MonoBehaviour
{

    // Lukas and Henric contributed 2026-03-05
    [SerializeField] GameObject player;
    [SerializeField] List<Transform> targetsToFollow;
    //[SerializeField] Transform targetToFollow;
    [SerializeField] float distanceToChangeTarget = 1f;
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float loseTargetRange = 10f;

    bool isFollowingPlayer;
    int currentTargetIndex;
    NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            isFollowingPlayer = true;
        
        }
        

        if (Vector3.Distance(transform.position, player.transform.position) > loseTargetRange)
        {
            isFollowingPlayer = false;
        }

        if(!isFollowingPlayer && agent.remainingDistance <= distanceToChangeTarget)
        {
            int randomIndex = Random.Range(0, targetsToFollow.Count);
            currentTargetIndex = randomIndex;
            agent.SetDestination(targetsToFollow[currentTargetIndex].position);
        }
        else if (isFollowingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }

            //agent.SetDestination(player.transform.position);
    }
}
