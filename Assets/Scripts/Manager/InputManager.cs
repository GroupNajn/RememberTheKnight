using Newtonsoft.Json.Linq;
using Unity.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public BindKey formatter;

    [Header("Input Actions")]
    public InputActionAsset inputActions;

    [Header("Player Inputs")]
    public PlayerInput[] playerInputs;

    [Header("Device Detection")]
    public bool usingGamepad = false;

    [Header("Settings")]
    [SerializeField] Toggle sprintToggle;
    public bool sprintToggleEnabled = false; // Set this to true to enable sprint toggle, false for hold-to-sprint

    private const string rebindKeys = "input_rebinds";
    private const string sprintToggleKey = "sprint_toggle";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (playerInputs != null && playerInputs.Length > 0 && playerInputs[0] != null)
        {
            inputActions = playerInputs[0].actions;
        }

        LoadBindings();
        LoadSettings();

    }

    private void Update()
    {
        DetectCurrentDevice();
    }

    private void DetectCurrentDevice()
    {
        bool usedGamepad = usingGamepad;

        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            usingGamepad = false;
        }

        if (Mouse.current != null && (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.middleButton.wasPressedThisFrame || Mouse.current.delta.value != Vector2.zero))
        {
            usingGamepad = false;
        }

        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            usingGamepad = true;
        }

        if (usingGamepad != usedGamepad)
        {
            Event_System.instance.OnDeviceChanged.Invoke();
        }

        ApplyControlScheme();
    }

    public void ApplyControlScheme()
    {
        if (playerInputs == null)
            return;

        foreach (var input in playerInputs)
        {
            if (input == null || !input.isActiveAndEnabled || !input.user.valid)
                continue;

            if (usingGamepad)
            {
                if (Gamepad.current == null)
                    continue;

                if (input.currentControlScheme != "Gamepad")
                {
                    input.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
                }
            }
            else
            {
                if (Keyboard.current == null || Mouse.current == null)
                    continue;

                if (input.currentControlScheme != "Keyboard&Mouse")
                {
                    input.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
                }
            }
        }
    }

    public void SaveAndApplyBindingsFrom(PlayerInput sourcePlayerInput)
    {
        if (sourcePlayerInput == null)
            return;

        SaveBindingsForInput(sourcePlayerInput);

        RefreshInput(sourcePlayerInput);

        foreach (var bindKey in FindObjectsByType<BindKey>(FindObjectsSortMode.None))
        {
            bindKey.UpdateBindingDisplay();
        }

        Debug.Log($"Bindings saved for {sourcePlayerInput.gameObject.name}");
    }

    private void SaveBindingsForInput(PlayerInput input)
    {
        if (input == null)
            return;

        string key = GetRebindKey(input);
        string json = input.actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        if (playerInputs == null)
            return;

        foreach (var input in playerInputs)
        {
            if (input == null)
                continue;

            string key = GetRebindKey(input);

            if (!PlayerPrefs.HasKey(key))
                continue;

            string json = PlayerPrefs.GetString(key);
            input.actions.LoadBindingOverridesFromJson(json);

            RefreshInput(input);
        }
    }

    public void ResetBindings()
    {
        if (playerInputs == null)
            return;

        foreach (var input in playerInputs)
        {
            if (input == null)
                continue;

            foreach (var map in input.actions.actionMaps)
            {
                map.RemoveAllBindingOverrides();
            }

            PlayerPrefs.DeleteKey(GetRebindKey(input));

            RefreshInput(input);
        }

        foreach (var bindKey in FindObjectsByType<BindKey>(FindObjectsSortMode.None))
        {
            bindKey.UpdateBindingDisplay();
        }

        Debug.Log("Bindings Reset");
    }

    private void RefreshInput(PlayerInput input)
    {
        if (input == null)
            return;

        foreach (var map in input.actions.actionMaps)
        {
            if (map.enabled)
            {
                map.Disable();
                map.Enable();
            }
        }
    }

    private string GetRebindKey(PlayerInput input)
    {
        return rebindKeys + "_" + input.gameObject.name;
    }

    public string SetInteractBinding()
    {
        InputAction interactAction = playerInputs[0].actions.FindAction("Interact");

        if (interactAction == null)
            return "F/Y";

        int bindingIndex = usingGamepad ? 1 : 0;

        if (bindingIndex >= interactAction.bindings.Count)
            return "F/Y";

        //string path = interactAction.bindings[bindingIndex].effectivePath;
        string key = interactAction.GetBindingDisplayString(bindingIndex);

        if (formatter != null)
            return formatter.GetEnglishBindingName(key);


        return key;
    }

    public string SetOpenBookBinding()
    {
        InputAction BookAction = playerInputs[0].actions.FindAction("OpenBook");

        if (BookAction == null)
            return "F/Y";

        int bindingIndex = usingGamepad ? 1 : 0;

        if (bindingIndex >= BookAction.bindings.Count)
            return "F/Y";

        //string path = interactAction.bindings[bindingIndex].effectivePath;
        string key = BookAction.GetBindingDisplayString(bindingIndex);

        if (formatter != null)
            return formatter.GetEnglishBindingName(key);


        return key;
    }

    public string SetHealBinding()
    {
        InputAction BookAction = playerInputs[0].actions.FindAction("Heal");

        if (BookAction == null)
            return "F/Y";

        int bindingIndex = usingGamepad ? 1 : 0;

        if (bindingIndex >= BookAction.bindings.Count)
            return "F/Y";

        //string path = interactAction.bindings[bindingIndex].effectivePath;
        string key = BookAction.GetBindingDisplayString(bindingIndex);

        if (formatter != null)
            return formatter.GetEnglishBindingName(key);


        return key;
    }

    public string SetHolsterBinding()
    {
        InputAction BookAction = playerInputs[0].actions.FindAction("Holster");

        if (BookAction == null)
            return "F/Y";

        int bindingIndex = usingGamepad ? 1 : 0;

        if (bindingIndex >= BookAction.bindings.Count)
            return "F/Y";

        //string path = interactAction.bindings[bindingIndex].effectivePath;
        string key = BookAction.GetBindingDisplayString(bindingIndex);

        if (formatter != null)
            return formatter.GetEnglishBindingName(key);


        return key;
    }

    public void SetSprintToggleValue(bool enabled)
    {
        sprintToggleEnabled = enabled;

        PlayerPrefs.SetInt(sprintToggleKey, sprintToggleEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(sprintToggleKey))
        {
            sprintToggleEnabled = PlayerPrefs.GetInt(sprintToggleKey,0) == 1;

            sprintToggle.SetIsOnWithoutNotify(sprintToggleEnabled);
        }
    }

}
