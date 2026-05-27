using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public BindKey formatter;

    [Header("Input Actions")]
    public InputActionAsset inputActions;

    [Header("Player Inputs")]
    public PlayerInput[] playerInputs;

    [Header("Device Detection")]
    public bool usingGamepad;

    private const string rebindKeys = "input_rebinds";

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



}
