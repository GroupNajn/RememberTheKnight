using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIManager : MonoBehaviour
{
    PlayerInput playerInput;
    PauseMenu pauseMenu;
    CardSelectionUI cardSelectionUI;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject cardSelectUI;
    [SerializeField] private GameObject interactUI;
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

            OpenPauseMenu(); // Show the pause menu
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
        CloseInteractiveUI(); // Hide the interact UI

    }

    public void OpenPauseMenu()
    {
        if (pauseMenuUI != null)
        {
            CloseInteractiveUI();
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
        Debug.Log("Opening Card Select UI");
        CloseInteractiveUI();
        cardSelectUI.SetActive(true);
        Debug.Log($"Card Select UI active: {cardSelectUI.activeInHierarchy}");

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
        CloseInteractiveUI();
        gameDeathScreenUI.SetActive(true);

        PlayerUIActive = true;
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused
        playerInput.enabled = false; // Disable player input when paused
    }

    public void ShowWinMenu()
    {
        CloseInteractiveUI();
        winMenuUI.SetActive(true);
    }

    public void OpenInteractiveUI()
    {
        interactUI.SetActive(true);
    }

    public void CloseInteractiveUI()
    {
        interactUI.SetActive(false);
    }
}
