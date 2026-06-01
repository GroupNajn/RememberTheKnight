using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class BossAddSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject enemyBossAdd;
    public BossAddSpawnerManager addSpawnerManager;


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
