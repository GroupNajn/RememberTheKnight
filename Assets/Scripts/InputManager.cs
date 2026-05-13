using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Input Actions")]
    public InputActionAsset inputActions;

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

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            usingGamepad = false;
        }

        // Gamepad
        if (Gamepad.current != null &&
            Gamepad.current.wasUpdatedThisFrame)
        {
            usingGamepad = true;
        }
    }

    public void SaveBindings()
    {
        string json = inputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(rebindKeys, json);
        PlayerPrefs.Save();

        Debug.Log("Bindings Saved");
    }

    public void LoadBindings()
    {
        if (!PlayerPrefs.HasKey(rebindKeys))
            return;

        string json = PlayerPrefs.GetString(rebindKeys);

        inputActions.LoadBindingOverridesFromJson(json);

        Debug.Log("Bindings Loaded");
    }

    public void ResetBindings()
    {
        foreach (var map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }

        PlayerPrefs.DeleteKey(rebindKeys);

        Debug.Log("Bindings Reset");
    }
}
