using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uses <see cref="BossAddSpawner"/> to spawn enemies in waves upp to a maximum
/// </summary>
/// <remarks>
/// <item>Used by <see cref="IntermissionPhaseAction"/></item>
/// <item>Author: Theo Johansson</item>
/// </remarks>
public class BossAddSpawnerManager : MonoBehaviour
{
    [SerializeField] List<BossAddSpawner> spawners;
    public int SpawnsRemaining { get; private set; }
    public int KilledAdds { get; private set; }
    int killsUntilNextWave = 0;
    public void SetSpawnTarget(int target)
    {
        SpawnsRemaining = target;
    }

    void Start()
    {
        spawners.ForEach(spawner => spawner.addSpawnerManager = this);
    }

    public void SpawnEnemies()
    {
        if (killsUntilNextWave <= 0)
        {
            foreach (var spawner in spawners)
            {
                if (SpawnsRemaining > 0)
                {
                    spawner.Spawn();
                    SpawnsRemaining--;
                    killsUntilNextWave++;
                }
            }
        }
    }

    public void RegisterDeath()
    {
        KilledAdds++;
        killsUntilNextWave--;
    }
}
