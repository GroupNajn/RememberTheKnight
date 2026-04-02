using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    GameObject pauseMenuUI;
    bool isPaused = false;
    PlayerInput playerInput;

    void Start()
    {
        pauseMenuUI = GameObject.Find("PauseMenu");

        if (pauseMenuUI == null)
        {
            Debug.Log("PauseMenu GameObject not found in the scene!");
        }
        else
        {
            pauseMenuUI.SetActive(false); // Ensure the pause menu is initially hidden
        }

        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    void OnPauseGame()
    {
        isPaused = !isPaused; // Toggle the pause state

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game by setting time scale to 0
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
            Cursor.visible = true; // Show the cursor when paused
            playerInput.enabled = false; // Disable player input when paused
        }
        else
        {
            Time.timeScale = 1f; // Resume the game by setting time scale back to 1
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
            Cursor.visible = false; // Hide the cursor when resuming
            playerInput.enabled = true; // Enable player input when resuming
        }

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(isPaused); // Show or hide the pause menu based on the pause state
        }
    }

    public void Resume()
    {
        isPaused = false; // Set the pause state to false
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide the pause menu
        }
    }

    public void LoadLobby()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        playerInput.enabled = true; // Enable player input when loading the main menu
        Debug.Log("LoadLobby called. Time scale set to 1 and player input enabled.");
        ThisSceneManager.Instance.LoadNewScene("LobbyMap"); // Load the lobby scene
        Debug.Log("Loading LobbyMap scene...");
        Debug.Log($"Current timescale: {Time.timeScale}, PlayerInput enabled: {playerInput.enabled}");
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}