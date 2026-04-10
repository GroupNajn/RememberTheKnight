using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    PlayerInput playerInput;
    //PlayerInput UIInput;
    PauseMenu pauseMenu;
    CardSelectionUI cardSelectionUI;

    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject startOptionMenuUI;
    [SerializeField] private GameObject pauseOptionMenuUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject cardSelectUI;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject gameDeathScreenUI;

    public bool UIMenuActive = true;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    private void Start()
    {
        //UIInput = GetComponentInChildren<PlayerInput>();
        pauseMenu = GetComponentInChildren<PauseMenu>();
        cardSelectionUI = GetComponentInChildren<CardSelectionUI>();

        GetComponentsInChildren<Transform>().ToList().ForEach(t =>
        {
            if (t.gameObject.name == "PauseMenu") pauseMenuUI = t.gameObject;
            else if (t.gameObject.name == "CardSelectUI") cardSelectUI = t.gameObject;
            else if (t.gameObject.name == "StartOptionMenuUI") startOptionMenuUI = t.gameObject;
            else if (t.gameObject.name == "StartOptionMenuUI") startOptionMenuUI = t.gameObject;
            else if (t.gameObject.name == "InteractUI") interactUI = t.gameObject;
            else if (t.gameObject.name == "WinMenu") winMenuUI = t.gameObject;
            else if (t.gameObject.name == "GameDeathScreen") gameDeathScreenUI = t.gameObject;
        });

        playerInput.enabled = false;
        //UIInput.enabled = false;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused
    }

    void OnPauseGame()
    {
        if (!UIMenuActive)
        {
            UIMenuActive = true;
            Time.timeScale = 0f; // Pause the game by setting time scale to 0
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
            Cursor.visible = true; // Show the cursor when paused
            playerInput.enabled = false; // Disable player input when paused

            OpenPauseMenu(); // Show the pause menu
        }
        else
        {
            if (startMenuUI.activeSelf)
                return;

            if (startOptionMenuUI.activeSelf)
            {
                CloseStartOptionMenu();
                OpenStartMenu();
                return;
            }

            if (pauseOptionMenuUI.activeSelf)
            {
                ClosePauseOptionMenu();
                OpenPauseMenu();
                return;
            }

            HideActiveUI();

        }

    }

    public void HideActiveUI()
    {
        UIMenuActive = false;
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming

        ClosePauseMenu(); // Hide the pause menu
        CloseCardSelectUI(); // Hide the card selection UI
        CloseInteractiveUI(); // Hide the interact UI
        CloseDeathScreen(); // Hide the death screen
    }

    public void OpenPauseMenu()
    {
        if (pauseMenuUI)
        {
            UIMenuActive = true;
            CloseInteractiveUI();
            pauseMenuUI.SetActive(true); // Show the pause menu

        }
    }

    public void ClosePauseMenu()
    {
        if (pauseMenuUI)
        {   
            UIMenuActive = false;
            pauseMenuUI.SetActive(false); // Hide the pause menu
        }
    }
    public void OpenStartMenu()
    {
        if (startMenuUI)
        {
            UIMenuActive = true;
            CloseInteractiveUI();
            startMenuUI.SetActive(true); // Show the pause menu

        }
    }

    public void CloseStartMenu()
    {
        if (startMenuUI)
        {
            startMenuUI.SetActive(false); // Hide the pause menu
        }
    }

    public void OpenStartOptionMenu()
    {
        if (startOptionMenuUI)
        {
            startOptionMenuUI.SetActive(true);
        }
    }

    public void CloseStartOptionMenu()
    {
        if (startOptionMenuUI)
        {
            startOptionMenuUI.SetActive(false);
            
        }
    }
    public void OpenPauseOptionMenu()
    {
        if (pauseOptionMenuUI)
        {

            pauseOptionMenuUI.SetActive(true);

        }
    }

    public void ClosePauseOptionMenu()
    {
        if (pauseMenuUI)
        {
            pauseOptionMenuUI.SetActive(false);
        }
    }

    public void OpenCardSelectUI()
    {
        Debug.Log("Opening Card Select UI");
        CloseInteractiveUI();
        cardSelectUI.SetActive(true);
        Debug.Log($"Card Select UI active: {cardSelectUI.activeInHierarchy}");

        UIMenuActive = true;
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused
        playerInput.enabled = false; // Disable player input when paused
    }

    public void CloseCardSelectUI()
    {
        cardSelectUI.SetActive(false);

        UIMenuActive = false;
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming
    }

    public void ShowDeathScreen()
    {
        CloseInteractiveUI();
        gameDeathScreenUI.SetActive(true);

        UIMenuActive = true;
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused
        playerInput.enabled = false; // Disable player input when paused
    }

    public void CloseDeathScreen()
    {
        gameDeathScreenUI.SetActive(false);
        UIMenuActive = false;
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming
    }

    public void ShowWinMenu()
    {
        CloseInteractiveUI();
        winMenuUI.SetActive(true);
    }

    public void OpenInteractiveUI()
    {
        if (interactUI)
        {
            interactUI.SetActive(true);
        }
    }

    public void CloseInteractiveUI()
    {
        if (interactUI)
            interactUI.SetActive(false);
    }

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //UIInput = GetComponentInChildren<PlayerInput>();
        pauseMenu = GetComponentInChildren<PauseMenu>();
        cardSelectionUI = GetComponentInChildren<CardSelectionUI>();

        Event_System.instance.OnPlayerDeath += ShowDeathScreen;
        Event_System.instance.OnWin += ShowWinMenu;

        GetComponentsInChildren<Transform>().ToList().ForEach(t =>
        {
            if (t.gameObject.name == "PauseMenu") pauseMenuUI = t.gameObject;
            else if (t.gameObject.name == "CardSelectUI") cardSelectUI = t.gameObject;
            else if (t.gameObject.name == "InteractUI") interactUI = t.gameObject;
            else if (t.gameObject.name == "WinMenu") winMenuUI = t.gameObject;
            else if (t.gameObject.name == "GameDeathScreen") gameDeathScreenUI = t.gameObject;
        });

        gameObject.SetActive(true);
        //UIInput.enabled = true;
    }
}