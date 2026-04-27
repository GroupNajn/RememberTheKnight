using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        PlayerKeepBetweenScene.Instance?.Teleport(transform);
    }
}