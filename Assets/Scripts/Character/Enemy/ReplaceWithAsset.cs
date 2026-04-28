using UnityEngine;

public class ReplaceWithAsset : MonoBehaviour, ITriggerable
{
    public void Trigger()
    {
        var asset = Instantiate(prefab, transform.position, Quaternion.identity);
        asset.layer = gameObject.layer;
        Destroy(gameObject);
    }
    [SerializeField] GameObject prefab;
}
