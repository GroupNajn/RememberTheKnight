using System.Linq;
using TMPro;
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
    [SerializeField] private BookUi bookUi; // Meike tbc
    [SerializeField] private PlayerCollection playerCollection; //meike tbc
    [SerializeField] private PlayerStats playerStats; //meike tbc

    [SerializeField] private GameObject backButtonUI;

    [Header("Menu UI")]
    [SerializeField] private GameObject backgrundUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject characterSelectUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionMenuUI;
    [SerializeField] public GameObject controllsUI;
    [SerializeField] public GameObject audioUI;
    [SerializeField] public GameObject videoUI;

    [Header("In game UI")]
    [SerializeField] private GameObject cardSelectUI;
    [SerializeField] private GameObject familySelectUI;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private GameObject cardShopUI;
    [SerializeField] private GameObject bookUI;

    [Header("Static UI")]
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject gameDeathScreenUI;
    [SerializeField] private GameObject soulUI;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject cupUI;
    [SerializeField] private GameObject weaponIconUI;


    public bool UIMenuActive = true;
    public bool timeScaleOn = true;


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

        HideActiveUI();

        playerInput.enabled = false;
        //UIInput.enabled = false;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused

        //backgrundUI.gameObject.SetActive(true);
        backButtonUI.gameObject.SetActive(false);

        UIMenuActive = true;
        startMenuUI.gameObject.SetActive(true);

    }

    void OnPauseGame()
    {
        if (!UIMenuActive)
        {
            if(playerStats.CurrentHealth <= 0)
                return;

            UIMenuActive = true;

            CheckUIState();
            OpenPauseMenu(); // Show the pause menu
        }
        else
        {
            if (startMenuUI.activeSelf)
                return;

            if (characterSelectUI.activeSelf)
                return;

            if (gameDeathScreenUI.activeSelf)
                return;

            if (optionMenuUI.activeSelf)
            {
                GoBackFromOptions();
                return;
            }

            if (controllsUI.activeSelf)
            {
                GoBackFromControlls();
                return;
            }

            if (audioUI.activeSelf)
            {
                GoBackFromAudio();
                return;
            }

            if (videoUI.activeSelf)
            {
                GoBackFromVideo();
                return;
            }



            HideActiveUI();

        }

    }

    void OnOpenBook()
    {
        if (!UIMenuActive)
        {
            OpenBookUI();
        }
        else
        {
            if (bookUI.activeSelf)
            {
                CloseBookUI();
            }
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
        CloseFamilySelectUI(); // Hide the Family selection UI
        CloseCardShopUI(); // Hide the Card Shop UI
        CloseInteractiveUI(); // Hide the interact UI
        CloseDeathScreen(); // Hide the death screen
        CloseCharacterSelectUI(); // Hide the character select UI
        CloseBookUI();

        CheckUIState();
        CheckTimeScaleUI(true);
    }

    public void CheckUIState()
    {
        if (!UIMenuActive)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
            Cursor.visible = false; // Hide the cursor when resuming
            playerInput.enabled = true; // Enable player input when resuming
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
            Cursor.visible = true; // Show the cursor when paused
            playerInput.enabled = false; // Disable player input when paused
        }
    }

    public void CheckTimeScaleUI(bool timeScaleOn)
    {
        if (timeScaleOn)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    // BACKGROUND
    public void CloseBackgroundUI()
    {
        backgrundUI.SetActive(false);
    }

    // BACK BUTTON
    public void OpenBackButtonUI()
    {
        backButtonUI.SetActive(true);
    }
    public void CloseBackButtonUI()
    {
        backButtonUI.SetActive(false);
    }


    // START MENU
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

    // PAUSE MENU
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
            pauseMenuUI.SetActive(false); // Hide the pause menu
        }
    }

    // CHR SELECET
    public void OpenCharacterSelectUI()
    {
        if (characterSelectUI)
        {
            UIMenuActive = true;
            characterSelectUI.SetActive(true);

        }
    }
    public void CloseCharacterSelectUI()
    {
        if (characterSelectUI)
        {
            characterSelectUI.SetActive(false);
        }
    }

    // OPTION MENU
    public void OpenOptionMenu()
    {
        if (optionMenuUI)
        {
            CloseInteractiveUI();
            optionMenuUI.SetActive(true);
            backButtonUI.SetActive(true);

            UIMenuActive = true;
            CheckUIState();
        }
    }

    public void CloseOptionMenu()
    {
        if (optionMenuUI)
        {
            optionMenuUI.SetActive(false);
        }
    }

    // CONTROLLS UI
    public void OpenControllsUI()
    {
        controllsUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
    }

    public void CloseControllsUI()
    {
        controllsUI.SetActive(false);
    }

    // AUDIO UI
    public void OpenAudioUI()
    {
        audioUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
    }

    public void CloseAudioUI()
    {
        audioUI.SetActive(false);
    }

    // AUDIO UI
    public void OpenVideoUI()
    {
        videoUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
    }

    public void CloseVideoUI()
    {
        videoUI.SetActive(false);
    }

    // CARD SELECT UI
    public void OpenCardSelectUI()
    {
        CloseInteractiveUI();
        cardSelectUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
        CheckTimeScaleUI(true);
    }

    public void CloseCardSelectUI()
    {
        cardSelectUI.SetActive(false);

        UIMenuActive = false;
        CheckUIState();
    }

    // FAMILY SELECT UI
    public void OpenFamilySelectUI()
    {
        CloseInteractiveUI();
        familySelectUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
        CheckTimeScaleUI(true);
    }

    public void CloseFamilySelectUI()
    {
        familySelectUI.SetActive(false);

        UIMenuActive = false;
        CheckUIState();
    }

    // CARD SHOP UI
    public void OpenCardShopUI()
    {
        CloseInteractiveUI();
        cardShopUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
        CheckTimeScaleUI(true);
    }

    public void CloseCardShopUI()
    {
        cardShopUI.SetActive(false);

        UIMenuActive = false;
        CheckUIState();
    }
    public void OpenBookUI()
    {
        CloseInteractiveUI();
        // bookUi.BuildInventory(playerCollection.ReturnPermanentCardCollection(), playerStats); // switch return permanent collection after script is done
        bookUi.BuildInventory(playerStats);
        bookUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
    }

    public void CloseBookUI()
    {
        Debug.Log("Closing Book UI");
        bookUI.SetActive(false);

        UIMenuActive = false;
        CheckUIState();
    }


    //DEATH UI
    public void ShowDeathScreen()
    {
        CloseInteractiveUI();
        gameDeathScreenUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
    }

    public void CloseDeathScreen()
    {
        gameDeathScreenUI.SetActive(false);
        UIMenuActive = false;
        CheckUIState();
    }

    // WIN UI
    public void OpenWinMenu()
    {
        CloseInteractiveUI();
        winMenuUI.SetActive(true);
        UIMenuActive = true;
        CheckUIState();
    }

    // INTERACT UI
    public void OpenInteractiveUI(string text)
    {
        if (interactUI)
        {
            interactUI.GetComponentInChildren<TMP_Text>().text = text;
            interactUI.SetActive(true);
        }
    }

    public void CloseInteractiveUI()
    {
        if (interactUI)
            interactUI.SetActive(false);
    }

    // SOULS UI
    public void OpenSoulUI()
    {
        soulUI.SetActive(true);
    }

    public void CloseSoulUI()
    {
        soulUI.SetActive(false);
    }

    // PLAYER BARS UI
    public void OpenPlayerBars()
    {
        healthBar.SetActive(true);
        staminaBar.SetActive(true);
    }

    public void ClosePlayerBars()
    {
        healthBar.SetActive(false);
        staminaBar.SetActive(false);
    }

    // CUP UI
    public void OpenCupUI()
    {
        cupUI.SetActive(true);
    }

    public void CloseCupUI()
    {
        cupUI.SetActive(false);
    }

    // WEAPON ICON UI
    public void OpenWeaponIconUI()
    {
        weaponIconUI.SetActive(true);
    }

    public void CloseWeaponIconUI()
    {
        weaponIconUI.SetActive(false);
    }


    // UI TOGGLE
    public void CloseUIOnMenuOpen()
    {
        CloseSoulUI();
        ClosePlayerBars();
        CloseCupUI();
        CloseWeaponIconUI();
    }

    public void OpenUIOnMenuClose()
    {
        OpenSoulUI();
        OpenPlayerBars();
        OpenCupUI();
        OpenWeaponIconUI();
    }


    public void GoBackFromOptions()
    {
        if (SceneManager.GetActiveScene().name == SceneData.Instance[0])
        {
            CloseOptionMenu();
            OpenStartMenu();

            CloseBackButtonUI();
        }
        else
        {
            CloseOptionMenu();
            OpenPauseMenu();
            
            CloseBackButtonUI();
            OpenUIOnMenuClose(); // SHOW BARS ETC
        }
    }
    public void GoBackFromControlls()
    {
        CloseControllsUI();
        OpenOptionMenu();
    }
    public void GoBackFromAudio()
    {
        CloseAudioUI();
        OpenOptionMenu();
    }
    public void GoBackFromVideo()
    {
        CloseVideoUI();
        OpenOptionMenu(); ;

    }
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
        Event_System.instance.OnWin += OpenWinMenu;

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