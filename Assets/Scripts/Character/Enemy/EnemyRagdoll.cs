using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyWeaponManager))]
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
        characterController = GetComponent<CharacterController>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        enemyWeaponManager = GetComponent<EnemyWeaponManager>();

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {

        animator.enabled = false;
        behaviorGraphAgent.enabled = false;
        foreach (var characterLimb in characterLimbs)
        {

            if (!characterLimb.CompareTag("Weapon"))
            {
                characterLimb.isKinematic = false;
                characterLimb.detectCollisions = true;
            }
        }
        characterRigidbody.isKinematic = true;
        capsuleCollider.enabled = false;
        characterController.enabled = false;
        navmeshAgent.enabled = false;
        enemyWeaponManager.DeactivateLeftDamageCollider();
        enemyWeaponManager.DeactivateRightDamageCollider();
        isRagdolled = true;
    }

    public void DisableRagdoll()
    {
        animator.enabled = true;
        behaviorGraphAgent.enabled = true;
        foreach (var characterLimb in characterLimbs)
        {
            if (!characterLimb.CompareTag("Weapon"))
            {
                characterLimb.isKinematic = true;
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
    private CapsuleCollider capsuleCollider;
    private CharacterController characterController;
    private NavMeshAgent navmeshAgent;
    private EnemyWeaponManager enemyWeaponManager;

    private bool isRagdolled = false;
}
