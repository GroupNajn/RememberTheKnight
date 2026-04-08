using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    bool isPaused = false;
    PlayerInput playerInput;
    PlayerUIManager playerUIManager;

    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerUIManager = GetComponentInParent<PlayerUIManager>();
    }
    public void Resume()
    {
        playerUIManager.PlayerUIActive = false; // Set the PlayerUIActive state to false
        isPaused = false; // Set the pause state to false
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming

        playerUIManager.ClosePauseMenu(); // Hide the pause menu
    }

    public void LoadLobby()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        playerInput.enabled = true; // Enable player input when loading the main menu
        Debug.Log("LoadLobby called. Time scale set to 1 and player input enabled.");
        ThisSceneManager.Instance.LoadScene("LobbyMap"); // Load the lobby scene
        Debug.Log("Loading LobbyMap scene...");
        Debug.Log($"Current timescale: {Time.timeScale}, PlayerInput enabled: {playerInput.enabled}");
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}