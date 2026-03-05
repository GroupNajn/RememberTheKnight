using UnityEngine;
using UnityEngine.AI;

public class Enemy_MVP_Ai : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed = 5f;
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
        agent.SetDestination(player.transform.position);
    }
}
