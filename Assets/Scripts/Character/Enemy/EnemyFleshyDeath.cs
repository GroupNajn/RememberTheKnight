using UnityEngine;

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
