using UnityEngine;

public class ReplaceWithAsset : MonoBehaviour, ITriggerable
{
    public void Trigger()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    [SerializeField] GameObject prefab;
}
