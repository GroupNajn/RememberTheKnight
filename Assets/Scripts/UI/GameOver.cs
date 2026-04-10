using UnityEngine;
using UnityEngine.InputSystem;

public class GameOver : MonoBehaviour
{
    public void LoadLobby()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        GlobalSceneManager.Instance.LoadScene("LobbyMap"); // Load the lobby scene

        PlayerUIManager.Instance.HideActiveUI(); // Hide any active UI elements

        PlayerStats playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        playerStats.Health = playerStats.MaxHealth; // Reset player's health to max
        playerStats.Heal(playerStats.MaxHealth); // Notify health change to update UI and other systems

        PlayerInput playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerInput.enabled = true; // Re-enable player input
    }
}