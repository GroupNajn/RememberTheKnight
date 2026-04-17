using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void LoadLobby()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("Loading lobby");
        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]); // Load the lobby scene
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode Mode)
    {
        UIManager.Instance.HideActiveUI(); // Hide any active UI elements
        UIManager.Instance.CheckUIState();

        PlayerInput playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerInput.enabled = true; // Re-enable player input

        SceneManager.sceneLoaded -= OnSceneLoaded; // Makes sure that it only happens after respawning
    }
}