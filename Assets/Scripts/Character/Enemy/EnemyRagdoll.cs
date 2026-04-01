using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyRagdoll : MonoBehaviour, ITriggerable

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
        DisableRagdoll();
    }



    // Update is called once per frame
    public void Trigger()
    {
        EnableRagdoll();
    }

    private void EnableRagdoll()
    {

        animator.enabled = false;
        behaviorGraphAgent.enabled = false;
        foreach (var characterLimb in characterLimbs)
        {
            characterLimb.isKinematic = false;
            characterLimb.detectCollisions = true;
        }

        characterRigidbody.useGravity = true;
        characterRigidbody.isKinematic = true;
        capsuleCollider.enabled = false;

        isRagdolled = true;
    }

    private void DisableRagdoll()
    {
        animator.enabled = true;
        behaviorGraphAgent.enabled = true;
        foreach (var characterLimb in characterLimbs)
        {
            characterLimb.isKinematic = true;
            characterLimb.detectCollisions = false;
        }

        //characterRigidbody.useGravity = true;
        characterRigidbody.isKinematic = true;
        characterRigidbody.detectCollisions = true;
        capsuleCollider.enabled = true;

        isRagdolled = false;
    }

    private Animator animator;
    private BehaviorGraphAgent behaviorGraphAgent;

    private Rigidbody characterRigidbody;
    private Rigidbody[] characterLimbs;
    private CharacterJoint[] characterJoints;
    private CapsuleCollider capsuleCollider;

    private bool isRagdolled = false;
}
