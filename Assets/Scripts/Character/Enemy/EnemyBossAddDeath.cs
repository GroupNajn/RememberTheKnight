using UnityEngine;

/// <summary>
/// Essantialy the same as <see cref="EnemyFleshyDeath"/> but also executes the <c>RegisterDeath</c> method on <see cref="BossAddSpawnerManager"/>
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[RequireComponent(typeof(EnemyRagdoll))]
public class EnemyBossAddDeath : MonoBehaviour, ITriggerable
{
    void Start()
    {
        ragdoll = GetComponent<EnemyRagdoll>();
    }
    public void Trigger()
    {
        if (ragdoll != null) ragdoll.EnableRagdoll();
        if (bossAddSpawnerManager) bossAddSpawnerManager.RegisterDeath();
    }
    [SerializeField] EnemyRagdoll ragdoll;
    public BossAddSpawnerManager bossAddSpawnerManager;
}
