using UnityEditor;
using UnityEngine;

public class BallistaSoulInstantiate : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] Transform spawnTransform;
    private Vector3 lastSpawnPosition;
    void Start()
    {
        
    }

    void Update()
    {
        if (spawnTransform != null)
        {
            lastSpawnPosition = spawnTransform.position;
        }
    }

    private void OnDestroy()
    {
        Instantiate(prefab, lastSpawnPosition, Quaternion.identity, this.transform);
    }
}
