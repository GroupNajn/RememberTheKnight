using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class BossAddSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject enemyBossAdd;
    public BossAddSpawnerManager addSpawnerManager;


    /// <summary>
    /// Plays a particle effect then spawns an enemy and registers the <see cref="BossAddSpawnerManager"/> to the <see cref="EnemyBossAddDeath"/>
    /// </summary>
    /// <remarks>Author: Theo Johansson</remarks>
    public void Spawn()
    {
        if (particles) particles.Play();
        var enemy = Instantiate(enemyBossAdd, transform.position, transform.rotation);
        // RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.bossSummonMinionEvent, enemy.gameObject);

        if (enemy.TryGetComponent<EnemyBossAddDeath>(out var enemyAdd))
        {
            enemyAdd.bossAddSpawnerManager = addSpawnerManager;
        }
    }
}
