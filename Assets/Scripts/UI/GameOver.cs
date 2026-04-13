using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void LoadLobby()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        GlobalSceneManager.Instance.LoadScene(SceneData.Instance[2]); // Load the lobby scene
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode Mode)
    {
        UIManager.Instance.HideActiveUI(); // Hide any active UI elements
        UIManager.Instance.CheckUIState();

        // Change to call revive method in PlayerStats
        PlayerStats playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        playerStats.Health = playerStats.MaxHealth; // Reset player's health to max
        playerStats.Heal(playerStats.MaxHealth); // Notify health change to update UI and other systems

        PlayerInput playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerInput.enabled = true; // Re-enable player input

        SceneManager.sceneLoaded -= OnSceneLoaded; // Makes sure that it only happens after respawning
    }
}