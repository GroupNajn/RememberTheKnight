using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        PlayerKeepBetweenScene.Instance?.Teleport(transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}