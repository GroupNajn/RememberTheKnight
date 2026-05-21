using UnityEditor;
using UnityEngine;

public class BallistaSoulInstantiate : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] Transform spawnTransform;
    [SerializeField] private EnemyLootProfile profile;
    private Vector3 lastSpawnPosition;
    private void Start()
    {
        profile = GetComponentInParent<EnemyLootProfile>();

        if (Event_System.instance != null)
            Event_System.instance.OnEnemyKilledNew += InstantiateChargedSoul;
    }

    private void OnDestroy()
    {
        if (Event_System.instance != null)
            Event_System.instance.OnEnemyKilledNew -= InstantiateChargedSoul;
    }

    private void Update()
    {
        if (spawnTransform != null)
            lastSpawnPosition = spawnTransform.position;
    }

    public void InstantiateChargedSoul(EnemyLootProfile sender, Vector3 throwAwayParameter)
    {
        if (sender != profile)
            return;

        Instantiate(prefab, lastSpawnPosition, Quaternion.identity);
    }
}
