using UnityEngine;

/// <summary>
/// Turns the enemy into a ragdoll when its health reaches zero
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[RequireComponent(typeof(EnemyRagdoll))]
public class EnemyFleshyDeath : MonoBehaviour, ITriggerable
{
    void Start()
    {
        ragdoll = GetComponent<EnemyRagdoll>();
    }
    public void Trigger()
    {
        if (ragdoll != null) ragdoll.EnableRagdoll();
    }
    [SerializeField] EnemyRagdoll ragdoll;
}
