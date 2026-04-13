using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        CharacterController playerCharacterController = player.GetComponent<CharacterController>();

        playerCharacterController.enabled = false;

        PlayerKeepBetweenScene.Instance?.Teleport(transform);

        playerCharacterController.enabled = true;
    }
}