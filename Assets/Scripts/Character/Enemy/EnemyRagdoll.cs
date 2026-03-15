using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyRagdoll : MonoBehaviour, ITriggerable

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        characterController = GetComponent<CharacterController>();
        characterRigidbody = GetComponent<Rigidbody>();
        characterLimbs = GetComponentsInChildren<Rigidbody>();
        characterJoints = GetComponentsInChildren<CharacterJoint>();
        DisableRagdoll();
    }



    // Update is called once per frame
    public void Trigger()
    {
        if (isRagdolled) { DisableRagdoll(); }
        else { EnableRagdoll(); }
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
        characterController.enabled = false;

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

        characterRigidbody.useGravity = true;
        characterRigidbody.isKinematic = true;
        characterController.enabled = true;

        isRagdolled = false;
    }

    private Animator animator;
    private BehaviorGraphAgent behaviorGraphAgent;

    private Rigidbody characterRigidbody;
    private Rigidbody[] characterLimbs;
    private CharacterJoint[] characterJoints;
    private CharacterController characterController;

    private bool isRagdolled = false;
}
