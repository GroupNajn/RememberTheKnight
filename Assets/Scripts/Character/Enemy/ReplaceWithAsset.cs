using UnityEngine;

public class ReplaceWithAsset : MonoBehaviour, ITriggerable
{
    public void Trigger()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(this);
    }
    [SerializeField] GameObject prefab;
}
