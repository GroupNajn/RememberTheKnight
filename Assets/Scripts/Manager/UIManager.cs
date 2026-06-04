using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // Created and edited by Lukas, Wilmer, Michaëla
    public static UIManager Instance { get; private set; }

    PlayerInput playerInput;
    PauseMenu pauseMenu;
    CardSelectionUI cardSelectionUI;
    [Header("Script References")]
    [SerializeField] private PlayerCollection playerCollection;
    [SerializeField] private PlayerStats playerStats; 
    [SerializeField] private InteractCameraHandler interactCameraHandler;

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
    [SerializeField] private GameObject bossCardSelectUI;
    [SerializeField] private GameObject familySelectUI;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private GameObject cardShopUI;
    [SerializeField] private GameObject bookUI;
    [SerializeField] private GameObject cardPickupUI;
    [SerializeField] private GameObject cardUnlockUI;
    [SerializeField] private GameObject lorePageUI;
    [SerializeField] private GameObject devSecretUI;

    [Header("Static UI")]
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject gameDeathScreenUI;
    [SerializeField] private GameObject soulUI;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject cupUI;
    [SerializeField] private GameObject weaponHUDUI;
    [SerializeField] private GameObject bookHUDUI;
    [SerializeField] private GameObject dialogueUI;

    public bool UIMenuActive = true;
    public bool timeScaleOn = true;
    public bool cameraTransitioning = false;


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
        pauseMenu = GetComponentInChildren<PauseMenu>();
        cardSelectionUI = GetComponentInChildren<CardSelectionUI>();

        playerInput.enabled = false;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
        Cursor.visible = true; // Show the cursor when paused

        backButtonUI.gameObject.SetActive(false);
        ClosePauseMenu();
        OpenStartMenu();
        Event_System.instance.OnLootPickedUp += OpenCardPickupUI;
        Event_System.instance.OnSacrificeSuccessful += OpenCardUnlockUI;

        Event_System.instance.OnLoadScenes += OnLoadScene;
        Event_System.instance.OnDeviceChanged += SetCursorBasedOnDevice;

    }

    void OnPauseGame()
    {
        if (!UIMenuActive)
        {
            if (GlobalSceneManager.Instance.isTransitioning)
                return;

            if (playerStats.CurrentHealth <= 0)
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

            if (cardPickupUI.activeSelf)
                return;

            if (cardUnlockUI.activeSelf)
                return;

            if (cameraTransitioning)
                return;

            if (lorePageUI.activeSelf)
                return;
            
            if (winMenuUI.activeSelf)
                return;

            // ADDED NEWLY

            if (cardShopUI.activeSelf)
            {
                //CloseCardShopUI();  
                return;
            }

            if (bookUI.activeSelf)
            {
                CloseBookUI();
                return;
            }

            // TO HERE

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

            if(dialogueUI.activeSelf)
            {
                CloseDialogueUI();
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

    /// <summary>
    /// Closes all currently active menus and restores gameplay control.
    /// Re-enables player input, gameplay UI and normal time scale.
    /// </summary>

    public void CheckUIState()
    {
        if (!UIMenuActive)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerInput.enabled = true;
        }
        else
        {
            Time.timeScale = 0f;

            playerInput.enabled = false;

            if (InputManager.Instance.usingGamepad)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    public void HideActiveUI()
    {
        UIMenuActive = false;
        Time.timeScale = 1f; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        playerInput.enabled = true; 

        ClosePauseMenu(); 
        CloseCardSelectUI(); 
        CloseFamilySelectUI(); 
        CloseCardShopUI(); 
        CloseInteractiveUI(); 
        CloseDeathScreen(); 
        CloseCharacterSelectUI(); 
        CloseLorePageUI(); 

        CheckUIState();
        SetTimeScale(true, 1f);
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

            startMenuUI.SetActive(true); // Show the pause menu

            if (backgrundUI == enabled)
                CloseInteractiveUI();

            if (backgrundUI == enabled)
                CloseBackground();

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


            if (backgrundUI == enabled)
                CloseBackground();
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
            CheckUIState();
            CloseUIOnMenuOpen();
            characterSelectUI.SetActive(true);

        }
    }

    public void CloseCharacterSelectUI()
    {
        if (characterSelectUI)
        {
            UIMenuActive = false;
            CheckUIState();
            OpenUIOnMenuClose();
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
            CheckTimeScaleDuringScene(0);

            OpenBackground();
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
        CheckTimeScaleDuringScene(0);
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
        CheckTimeScaleDuringScene(0);
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
        CheckTimeScaleDuringScene(0);
    }

    public void CloseVideoUI()
    {
        videoUI.SetActive(false);
    }

    // CARD SELECT UI
    public void OpenCardSelectUI()
    {
        if (interactUI == enabled)
            CloseInteractiveUI();

        CloseUIOnMenuOpen();

        cardSelectUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
        SetTimeScale(true, 1f);
    }

    public void CloseCardSelectUI()
    {
        OpenUIOnMenuClose();
        cardSelectUI.SetActive(false);

        UIMenuActive = false;
        cameraTransitioning = false;
        CheckUIState();
        interactCameraHandler.InteractCamReset();
    }

    // BOSS CARD SELECT UI
    public void OpenBossCardSelectUI()
    {
        if (interactUI == enabled)
            CloseInteractiveUI();

        CloseUIOnMenuOpen();

        bossCardSelectUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
        SetTimeScale(true, 1f);
    }

    public void CloseBossCardSelectUI()
    {
        OpenUIOnMenuClose();
        bossCardSelectUI.SetActive(false);

        UIMenuActive = false;
        cameraTransitioning = false;
        CheckUIState();
        interactCameraHandler.InteractCamReset();
    }

    // FAMILY SELECT UI
    public void OpenFamilySelectUI()
    {
        if (interactUI == enabled)
            CloseInteractiveUI();

        CloseUIOnMenuOpen();

        familySelectUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
        SetTimeScale(true, 1f);
    }

    public void CloseFamilySelectUI()
    {
        OpenUIOnMenuClose();
        familySelectUI.SetActive(false);

        UIMenuActive = false;
        cameraTransitioning = false;
        CheckUIState();

        interactCameraHandler.InteractCamReset();
    }

    // CARD PICKUP UI
    public void OpenCardPickupUI(Loot loot)
    {
        CardPickupUI pickupUI = cardPickupUI.GetComponent<CardPickupUI>();

        if (loot == null) return;

        if (loot.gameObject.TryGetComponent<Card>(out Card card) && !playerCollection.CardIsPickedUp(card.CardData))
        {
            if (pickupUI.SetCardData(card.CardData))
            {
                cardPickupUI.SetActive(true);
                UIMenuActive = true;
                CheckUIState();
            }
        }
        else
            playerCollection.PickupLoot(loot);
    }

    public void OpenCardUnlockUI(CardData card)
    {
        CardUnlockUI unlockUI = cardUnlockUI.GetComponent<CardUnlockUI>();

        if (card == null) return;

        if (unlockUI.SetCardData(card))
        {
            UIMenuActive = true;
            CheckUIState();
            cardUnlockUI.SetActive(true);
            CloseInteractiveUI();
        }
    }

    // CARD SHOP UI
    public void OpenCardShopUI()
    {
        CloseInteractiveUI();
        cardShopUI.SetActive(true);

        cameraTransitioning = false;
        CheckUIState();
        SetTimeScale(true, 1f);
    }

    public void CloseCardShopUI()
    {
        cardShopUI.SetActive(false);

        UIMenuActive = false;
        cameraTransitioning = false;
        CheckUIState();
        interactCameraHandler.InteractCamReset();
    }

    public void OpenBookUI()
    {
        CloseInteractiveUI();
        CloseUIOnMenuOpen();
        bookUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
    }

    public void CloseBookUI()
    {
        BookUi bookScript = bookUI.GetComponent<BookUi>();
        if (!bookScript.isAnimating)
        {
            bookScript.isAnimating = true;
            bookScript.AnimateClose(() =>
            {
                bookScript.AnimateMove(() =>
                {
                    StartCoroutine(bookScript.AnimateSize(() =>
                    {
                        bookScript.isAnimating = false;
                        OpenUIOnMenuClose();
                        bookUI.SetActive(false);

                        UIMenuActive = false;
                        CheckUIState();

                    }, true));
                }, true);
            });
        }
    }

    public void OpenInventoryAfterPurchase()
    {
        StartCoroutine(DelayedAction(0.5f, () =>
        {
            OpenBookUI();
            bookUI.GetComponent<BookUi>().OpenTab(BookUi.BookTabEnum.Cards);
        }));
    }

    // LORE PAGE UI
    public void OpenLorePageUI()
    {
        CloseInteractiveUI();
        CloseUIOnMenuOpen();
        lorePageUI.SetActive(true);
        
        UIMenuActive = true;
        CheckUIState();
    }

    public void CloseLorePageUI()
    {
        OpenUIOnMenuClose();
        lorePageUI.SetActive(false);

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
    public void OpenWeaponHUDUI()
    {
        weaponHUDUI.SetActive(true);
    }

    public void CloseWeaponHUDUI()
    {
        weaponHUDUI.SetActive(false);
    }

    // BOOK HUD UI
    public void OpenBookHUDUI()
    {
        bookHUDUI.SetActive(true);
    }

    public void CloseBookHUDUI()
    {
        bookHUDUI.SetActive(false);
    }

    // UI TOGGLE
    public void CloseUIOnMenuOpen()
    {
        CloseSoulUI();
        ClosePlayerBars();
        CloseCupUI();
        CloseWeaponHUDUI();
        CloseBookHUDUI();
    }

    public void OpenUIOnMenuClose()
    {
        OpenSoulUI();
        OpenPlayerBars();
        OpenCupUI();
        OpenWeaponHUDUI();
        OpenBookHUDUI();
    }

    // BACKGROUND
    public void OpenBackground()
    {
        backgrundUI.gameObject.SetActive(true);
    }

    public void CloseBackground()
    {
        backgrundUI.gameObject.SetActive(false);
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

    /// <summary>
    /// Opens the dialogue interface and begins displaying
    /// the supplied dialogue sequence.
    /// </summary>
    /// <param name="dialogueLines">Dialogue lines to display.</param>
    public void OpenDialogueUI(Dialogue[] dialogueLines)
    {
        CloseInteractiveUI();
        CloseUIOnMenuOpen();

        dialogueUI.SetActive(true);

        UIMenuActive = true;

        CheckUIState();
        SetTimeScale(true, 1f);

        dialogueUI.GetComponent<DialogueUI>().StartDialogue(dialogueLines);
    }
    public void CloseDialogueUI()
    {
        OpenUIOnMenuClose();
        dialogueUI.SetActive(false);

        UIMenuActive = false;
        CheckUIState();
    }
    public void OpenDevSecretUI()
    {

        CloseInteractiveUI();
        CloseUIOnMenuOpen();
        devSecretUI.SetActive(true);

        UIMenuActive = true;
        CheckUIState();
    }
    public void CloseDevSecretUI()
    {
        OpenUIOnMenuClose();
        devSecretUI.SetActive(false);

        UIMenuActive = false;
        CheckUIState();
    }

    // NEW METHODS
    public void OpenUI(GameObject UIelement)
    {
        UIelement.SetActive(true);
    }

    public void CloseUI(GameObject UIelement)
    {
        UIelement.SetActive(false);
    }

    public void ToggleUI(GameObject openUI, GameObject closeUI)
    {
        OpenUI(openUI);
        CloseUI(closeUI);
    }

    public void SetUIState(bool active)
    {
        UIMenuActive = active;

        // AS OF RN THIS WOULD DO NOTHING AND ONLY BE A DEBUGGER
    }

    public void SetCursorState(bool visible, bool locked)
    {
        if (InputManager.Instance.usingGamepad) // USING GAMEPAD SHOW ALWAYS HIDE AND LOCK IT
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
        }

        Cursor.visible = visible;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.Confined; // MOUSE IS LOCKED OR CONFIED TO ONE SCREEN (MAYBE CHANGE THIS TO NONE)
    }

    public void SetTimeScale(bool timeScaleOn, float timeScale)
    {
        if (timeScaleOn)
        {
            Time.timeScale = timeScale;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    // THINGS I DONT KNOW HOW TO REFECTOR YET
    public void CheckTimeScaleDuringScene(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().name == SceneData.Instance[sceneIndex])
        {
            SetTimeScale(true, 1f);
        }
    }
    void OnLoadScene()
    {
        // Close menues when the screen is black
        if (SceneManager.GetActiveScene().name == SceneData.Instance[1])
        {
            CloseStartMenu();
            //uiManager.CloseBackgroundUI();
            UIMenuActive = false;

            OpenCharacterSelectUI();

            // TURNS OFF THE SOULS CANVAS WHEN IN CHARCATER SELECT
            CloseSoulUI();
            CloseCupUI();
        }
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
    IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    // THIS THAT WILL GET REMOVED
    void SetCursorBasedOnDevice()
    {
        if (InputManager.Instance.usingGamepad)
        {
            Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor when paused
            Cursor.visible = false; // Show the cursor when paused
        }
        else
        {
            if (UIMenuActive)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor when paused
                Cursor.visible = true; // Show the cursor when paused
            }
        }
    }


}