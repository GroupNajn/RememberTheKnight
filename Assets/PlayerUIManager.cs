using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIManager : MonoBehaviour
{
    PlayerInput playerInput;
    PauseMenu pauseMenu;
    CardSelectionUI cardSelectionUI;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject cardSelectUI;
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject gameDeathScreenUI;

    public bool PlayerUIActive = false;

    private void Awake()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        pauseMenu = GetComponent<PauseMenu>();
        cardSelectionUI = GetComponent<CardSelectionUI>();
    }

    private void Start()
    {
        Event_System.instance.OnPlayerDeath += ShowDeathScreen;
        Event_System.instance.OnWin += ShowWinMenu;
    }

    void OnPauseGame()
    {
        if (!PlayerUIActive)
        {
            PlayerUIActive = true;
            Time.timeScale = 0f; // Pause the game by setting time scale to 0
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
            Cursor.visible = true; // Show the cursor when paused
            playerInput.enabled = false; // Disable player input when paused

            ShowPauseMenu(); // Show the pause menu
        }
        else
        {
            PlayerUIActive = false;
            Time.timeScale = 1f; // Resume the game by setting time scale back to 1
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
            Cursor.visible = false; // Hide the cursor when resuming
            playerInput.enabled = true; // Enable player input when resuming

            HideActiveUI();

        }

    }

    void HideActiveUI()
    {
        ClosePauseMenu(); // Hide the pause menu
        CloseCardSelectUI(); // Hide the card selection UI
    }

    public void ShowPauseMenu()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // Show the pause menu
        }
    }

    public void ClosePauseMenu()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide the pause menu
        }
    }

    public void OpenCardSelectUI()
    {


        cardSelectUI.SetActive(true);

        PlayerUIActive = true;
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused
        playerInput.enabled = false; // Disable player input when paused
    }

    public void CloseCardSelectUI()
    {
        cardSelectUI.SetActive(false);

        PlayerUIActive = false;
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming
    }

    public void ShowDeathScreen()
    {
        gameDeathScreenUI.SetActive(true);

        PlayerUIActive = true;
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused
        playerInput.enabled = false; // Disable player input when paused
    }

    public void ShowWinMenu()
    {
        winMenuUI.SetActive(true);
    }
}
