using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    string returnButtonDefaultText;
    [SerializeField] string returnButtonLobbyText;
    TMP_Text returnButtonText;

    PlayerInput playerInput;
    UIManager uiManager;

    private void Awake()
    {
        GetText();
        returnButtonDefaultText = returnButtonText.text; // Store the default text of the return button

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event   
    }

    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GetComponentInParent<UIManager>();

        gameObject.SetActive(false); // Ensure the pause menu is initially inactive 
    }

    void GetText()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "ReturnToLobbyButtonText (TMP)")
                returnButtonText = child.GetComponent<TMP_Text>();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneData.Instance[2])
        {
            returnButtonText.text = returnButtonLobbyText; // Update the return button text for the lobby scene

            // Change to call revive method in PlayerStats
            PlayerStats playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
            playerStats.Health = playerStats.MaxHealth; // Reset player's health to max
            playerStats.Heal(playerStats.MaxHealth); // Notify health change to update UI and other systems
        }
        else
        {
            returnButtonText.text = returnButtonDefaultText; // Reset to default text for other scenes
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming

        uiManager.ClosePauseMenu(); // Hide the pause menu
    }

    public void OpenPauseOptions()
    {
        //uiManager.OpenPauseOptionMenu();
        uiManager.ClosePauseMenu();
    }

    public void LoadLobby()
    {
        if (SceneManager.GetActiveScene().name == SceneData.Instance[2])
        {
            QuitGame(); // Quit the application if already in the lobby scene
            return; // Not sure if needed, but just to be safe, we return after quitting
        }

        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]);

        UIManager.Instance.HideActiveUI();
        UIManager.Instance.CheckUIState();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game..."); // Log a message to indicate the quit action
        Application.Quit(); // Quit the application
    }
}