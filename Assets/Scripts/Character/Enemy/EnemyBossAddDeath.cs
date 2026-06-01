using UnityEngine;

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
