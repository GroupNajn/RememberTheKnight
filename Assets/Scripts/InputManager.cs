using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Input Actions")]
    public InputActionAsset inputActions;

    [Header("Player Input")]
    public PlayerInput playerInput;

    [Header("Device Detection")]
    public bool usingGamepad;

    private const string rebindKeys = "input_rebinds";

    private void Awake()
    {
        // Singleton
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

        if (playerInput != null)
        {
            inputActions = playerInput.actions;
        }

        LoadBindings();
    }

    private void Update()
    {
        DetectCurrentDevice();
    }

    void DetectCurrentDevice()
    {
        // Keyboard / Mouse
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            usingGamepad = false;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) // MAY NEED TO ADD MORE MOUSE BUTTONS
        {
            usingGamepad = false;
        }

        // Gamepad
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            usingGamepad = true;
        }
    }
    public void SaveBindings()
    {
        if (inputActions == null)
            return;

        string json = inputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(rebindKeys, json);
        PlayerPrefs.Save();

        Debug.Log("Bindings Saved");
    }

    public void LoadBindings()
    {
        if (inputActions == null)
            return;

        if (!PlayerPrefs.HasKey(rebindKeys))
            return;

        string json = PlayerPrefs.GetString(rebindKeys);

        inputActions.LoadBindingOverridesFromJson(json);

        Debug.Log("Bindings Loaded");
    }
}