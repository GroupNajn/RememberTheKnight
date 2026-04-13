using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(BehaviorGraphAgent), typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody), typeof(CharacterController), typeof(CapsuleCollider))]
[System.Serializable]
public class EnemyRagdoll : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        characterRigidbody = GetComponent<Rigidbody>();
        characterLimbs = GetComponentsInChildren<Rigidbody>();
        characterJoints = GetComponentsInChildren<CharacterJoint>();
        characterController = GetComponent<CharacterController>();
        navmeshAgent = GetComponent<NavMeshAgent>();


        DisableRagdoll();
    }

    public void EnableRagdoll()
    {

        animator.enabled = false;
        behaviorGraphAgent.enabled = false;
        foreach (var characterLimb in characterLimbs)
        {
            if (!characterLimb.CompareTag("Weapon"))
               { characterLimb.isKinematic = false;
                characterLimb.detectCollisions = true;
            }
        }
        characterRigidbody.isKinematic = true;
        capsuleCollider.enabled = false;
        characterController.enabled = false;
        navmeshAgent.enabled = false;
        isRagdolled = true;
    }

    public void DisableRagdoll()
    {
        animator.enabled = true;
        behaviorGraphAgent.enabled = true;
        foreach (var characterLimb in characterLimbs)
        {
            if(!characterLimb.CompareTag("Weapon"))
            {characterLimb.isKinematic = true;
                characterLimb.detectCollisions = false;
            }
        }
        characterRigidbody.isKinematic = true;
        characterRigidbody.detectCollisions = true;
        capsuleCollider.enabled = true;
        characterController.enabled = true;
        navmeshAgent.enabled = true;
        isRagdolled = false;
    }

    private Animator animator;
    private BehaviorGraphAgent behaviorGraphAgent;

    private Rigidbody characterRigidbody;
    private Rigidbody[] characterLimbs;
    private CharacterJoint[] characterJoints;
    private CapsuleCollider capsuleCollider;
    private CharacterController characterController;
    private NavMeshAgent navmeshAgent;

    private bool isRagdolled = false;
}
