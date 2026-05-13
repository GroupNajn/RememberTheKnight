using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Input Actions")]
    public InputActionAsset inputActions;

<<<<<<< Updated upstream
    [Header("Player Input")]
    public PlayerInput playerInput;

=======
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        if(playerInput != null)
        {
            inputActions = playerInput.actions;
        }

=======
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) // MAY NEED TO ADD MORE MOUSE BUTTONS
=======
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
>>>>>>> Stashed changes
        {
            usingGamepad = false;
        }

        // Gamepad
<<<<<<< Updated upstream
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
=======
        if (Gamepad.current != null &&
            Gamepad.current.wasUpdatedThisFrame)
>>>>>>> Stashed changes
        {
            usingGamepad = true;
        }
    }

    public void SaveBindings()
    {
<<<<<<< Updated upstream
        if (inputActions == null)
            return;

=======
>>>>>>> Stashed changes
        string json = inputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(rebindKeys, json);
        PlayerPrefs.Save();

        Debug.Log("Bindings Saved");
    }

    public void LoadBindings()
    {
<<<<<<< Updated upstream
        if (inputActions == null)
            return;

=======
>>>>>>> Stashed changes
        if (!PlayerPrefs.HasKey(rebindKeys))
            return;

        string json = PlayerPrefs.GetString(rebindKeys);

        inputActions.LoadBindingOverridesFromJson(json);

        Debug.Log("Bindings Loaded");
    }

    public void ResetBindings()
    {
<<<<<<< Updated upstream
        if (inputActions == null)
            return;

=======
>>>>>>> Stashed changes
        foreach (var map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }

        PlayerPrefs.DeleteKey(rebindKeys);

<<<<<<< Updated upstream
        foreach (var bindKey in FindObjectsByType<BindKeys>(FindObjectsSortMode.None))
        {
            bindKey.UpdateBindingDisplay();
        }

=======
>>>>>>> Stashed changes
        Debug.Log("Bindings Reset");
    }
}
