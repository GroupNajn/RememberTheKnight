using Unity.Behavior;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BehaviorGraphAgent))]
public class Enemy_Damage : MonoBehaviour, IDamageable
{
    // Made by Lukas and Anton A 2026-03-06
    [field: SerializeField] public int MaxHealth { get; set; }
    [HideInInspector] public int Health { get; set; }
    [HideInInspector] public bool CanTakeDamage { get; set; } = true;
    private Animator animator;
    private BehaviorGraphAgent behaviorGraphAgent;

    private Rigidbody characterRigidbody;
    private Rigidbody[] characterLimbs;
    private CharacterJoint[] characterJoints;

    [SerializeField] GameObject skeletonPilePrefab = null;

    float damageCooldownTimer = 1;
    [SerializeField] float damageCooldown = 1;

    public void TakeDamage(int damage)
    {
        if (CanTakeDamage && Health > 0)
        {
            Debug.Log($"Taking damage{damage}");

            Health -= damage;
            Debug.Log($"Health {Health}/{MaxHealth}");
            CanTakeDamage = false;
            if (Health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        if (skeletonPilePrefab != null)
        {
            Instantiate(skeletonPilePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            animator.enabled = false;
            behaviorGraphAgent.enabled = false;
            EnableRagdoll();
        }
    }

    // TO BE REMOVED OR CHANGED
    void Update()
    {
        if (!CanTakeDamage)
        {
            damageCooldownTimer -= Time.deltaTime;
            if (damageCooldownTimer <= 0)
            {
                CanTakeDamage = true;
                damageCooldownTimer = damageCooldown;
            }
        }
    }

    private void Start()
    {
        Health = MaxHealth;
        animator = GetComponent<Animator>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        characterRigidbody = GetComponent<Rigidbody>();
        characterLimbs = GetComponentsInChildren<Rigidbody>();
        characterJoints = GetComponentsInChildren<CharacterJoint>();
        DisableRagdoll();
        // foreach (var joint in characterJoints)
        // {
        //     joint.breakForce = 0;
        // }


    }

    private void EnableRagdoll()
    {
        foreach (var characterLimb in characterLimbs)
        {
            characterLimb.isKinematic = false;
            characterLimb.detectCollisions = true;
        }

        characterRigidbody.useGravity = true;
        characterRigidbody.isKinematic = true;
    }

    private void DisableRagdoll()
    {
        foreach (var characterLimb in characterLimbs)
        {
            characterLimb.isKinematic = true;
            characterLimb.detectCollisions = false;
        }

        characterRigidbody.useGravity = true;
        characterRigidbody.isKinematic = true;
    }
}