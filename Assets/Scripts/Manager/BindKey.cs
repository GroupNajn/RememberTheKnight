using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class BindKey : MonoBehaviour
{
    [Header("Action")]
    public InputActionReference actionReference;

    [Header("Binding")]
    public int bindingIndex;

    [Header("UI")]
    public TextMeshProUGUI bindingText;

    [Header("Device Restrictions")]
    public bool keyboardMouseBinding;
    public bool gamepadBinding;

    [Header("Target Player Input")]
    public PlayerInput targetPlayerInput;

    private InputAction action;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        if (actionReference == null)
        {
            Debug.LogError($"Missing InputActionReference on {gameObject.name}");
            return;
        }

        if (targetPlayerInput == null && InputManager.Instance != null)
        {
            targetPlayerInput = InputManager.Instance.playerInputs[0];
        }

        if (targetPlayerInput != null)
        {
            string mapName = actionReference.action.actionMap.name;
            string actionName = actionReference.action.name;
            string actionPath = mapName + "/" + actionName;

            action = targetPlayerInput.actions.FindAction(actionPath, false);

            if (action == null)
            {
                Debug.LogError($"Could not find action '{actionPath}' in {targetPlayerInput.gameObject.name}");
                return;
            }
        }
        else
        {
            action = actionReference.action;
        }

        if (bindingText == null)
            bindingText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateBindingDisplay();
    }

    private void OnDisable()
    {
        rebindingOperation?.Dispose();
        rebindingOperation = null;
    }

    public void StartRebind()
    {
        if (action == null || bindingText == null)
            return;

        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            Debug.LogError($"Invalid binding index {bindingIndex} on {gameObject.name}");
            bindingText.text = "Invalid";
            return;
        }

        rebindingOperation?.Dispose();

        action.Disable();
        bindingText.text = "...";

        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Mouse>/scroll")
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Pointer>/delta")
            .WithCancelingThrough("<Keyboard>/escape");

        if (keyboardMouseBinding)
        {
            rebindingOperation.WithControlsExcluding("<Gamepad>");
        }
        else if (gamepadBinding)
        {
            rebindingOperation.WithControlsHavingToMatchPath("<Gamepad>");
        }

        rebindingOperation.OnCancel(operation =>
        {
            action.Enable();

            operation.Dispose();
            rebindingOperation = null;

            UpdateBindingDisplay();
        });

        rebindingOperation.OnComplete(operation =>
        {
            string newBinding = action.bindings[bindingIndex].effectivePath;

            bool invalidBinding = false;

            if (keyboardMouseBinding)
            {
                invalidBinding = !newBinding.StartsWith("<Keyboard>") && !newBinding.StartsWith("<Mouse>");
            }
            else if (gamepadBinding)
            {
                invalidBinding = !newBinding.StartsWith("<Gamepad>");
            }

            if (invalidBinding)
            {
                Debug.LogWarning($"Invalid binding for this slot: {newBinding}");
                action.RemoveBindingOverride(bindingIndex);
            }
            else if (IsDuplicateBinding(newBinding))
            {
                Debug.LogWarning($"Duplicate binding detected: {newBinding}");
                action.RemoveBindingOverride(bindingIndex);
            }

            action.Enable();

            operation.Dispose();
            rebindingOperation = null;

            UpdateBindingDisplay();

            if (InputManager.Instance != null)
                InputManager.Instance.SaveAndApplyBindingsFrom(targetPlayerInput);
        });

        rebindingOperation.Start();
    }

    private bool IsDuplicateBinding(string newBinding)
    {
        foreach (var map in action.actionMap.asset.actionMaps)
        {
            foreach (var otherAction in map.actions)
            {
                for (int i = 0; i < otherAction.bindings.Count; i++)
                {
                    if (otherAction == action && i == bindingIndex)
                        continue;

                    if (otherAction.bindings[i].effectivePath == newBinding)
                        return true;
                }
            }
        }

        return false;
    }

    public void UpdateBindingDisplay()
    {
        if (action == null || bindingText == null)
            return;

        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            bindingText.text = "Invalid";
            return;
        }

        string path = action.bindings[bindingIndex].effectivePath;

        bindingText.text = GetEnglishBindingName(path);

    }

    public string GetEnglishBindingName(string path)
    {
        if (string.IsNullOrEmpty(path))
            return "Invalid";

        if (path.StartsWith("<Keyboard>"))
        {
            string key = path.Replace("<Keyboard>/", "");

            return key switch
            {
                "space" => "Space",
                "enter" => "Enter",
                "escape" => "Escape",
                "leftShift" => "Left Shift",
                "rightShift" => "Right Shift",
                _ => FormatKeyboardKey(key)
            };
        }

        if (path.StartsWith("<Mouse>/"))
        {
            string mouse = path.Replace("<Mouse>/", "");

            return mouse switch
            {
                "leftButton" => "Mouse Left",
                "rightButton" => "Mouse Right",
                "middleButton" => "Mouse Middle",
                "forwardButton" => "Mouse Forward",
                "backButton" => "Mouse Back",
                _ => FormatKeyboardKey(mouse)
            };
        }

        if (path.StartsWith("<Gamepad>/"))
        {
            string button = path.Replace("<Gamepad>/", "");

            return button switch
            {
                "buttonSouth" => GetGamepadFaceButton("South"),
                "buttonEast" => GetGamepadFaceButton("East"),
                "buttonWest" => GetGamepadFaceButton("West"),
                "buttonNorth" => GetGamepadFaceButton("North"),

                "leftShoulder" => GetShoulderButton(true),
                "rightShoulder" => GetShoulderButton(false),
                "leftTrigger" => GetTriggerButton(true),
                "rightTrigger" => GetTriggerButton(false),

                "leftStickPress" => "Left Stick",
                "rightStickPress" => "Right Stick",

                "dpad/up" => "D-Pad Up",
                "dpad/down" => "D-Pad Down",
                "dpad/left" => "D-Pad Left",
                "dpad/right" => "D-Pad Right",

                _ => FormatKeyboardKey(button)
            };
        }
        return path;
    }


    public string FormatKeyboardKey(string key) //MAKES NUMBER LOOK BETTER
    {
        if (key.StartsWith("digit"))
            return key.Replace("digit", ""); // MAKE DIGIT KEYS ONLY SAY THE NUMBER

        if (key.StartsWith("numpad"))
            return "Numpad" + key.Replace("numpad", ""); // MAKES NUMPAD KEYS LOOK BETYER

        return FormatName(key);
    }


    public string FormatName(string value) // MAKES TEXT LOOK BETTER
    {
        if (string.IsNullOrEmpty(value))
            return "Invalid";

        string result = "";

        for (int i = 0; i < value.Length; i++) // TURNS CAMEL CASE INTO NORMAL TEXT
        {
            char c = value[i];

            if (i > 0 && char.IsUpper(c)) // IFIT'S AN UPPERCASE LETTER AND NOT THE FIRST CHARACTER, ADD A SPACE BEFORE IT
                result += " ";
            result += c;
        }

        return char.ToUpper(result[0]) + result.Substring(1); // MAKE FIRST LETTER UPPERCASE
    }

    public string GetGamepadFaceButton(string direction)
    {
        ControllerType type = GetCurrentControllerType();

        if (type == ControllerType.PlayStation)
        {
            return direction switch
            {
                "South" => "Cross",
                "East" => "Circle",
                "West" => "Square",
                "North" => "Triangle",
                _ => direction
            };
        }

        if (type == ControllerType.Xbox)
        {
            return direction switch
            {
                "South" => "A",
                "East" => "B",
                "West" => "X",
                "North" => "Y",
                _ => direction
            };
        }

        return direction switch
        {
            "South" => "A",
            "East" => "B",
            "West" => "X",
            "North" => "Y",
            _ => direction
        };

    }

    public string GetShoulderButton(bool left)
    {
        ControllerType type = GetCurrentControllerType();

        if (type == ControllerType.PlayStation)
        {
            return left ? "L1" : "R1";
        }

        if (type == ControllerType.Xbox)
        {
            return left ? "LB" : "RB";
        }

        return left ? "LB" : "RB";

    }

    public string GetTriggerButton(bool left)
    {
        ControllerType type = GetCurrentControllerType();

        if (type == ControllerType.PlayStation)
        {
            return left ? "L2" : "R2";
        }
        if (type == ControllerType.Xbox)
        {
            return left ? "LT" : "RT";
        }

        return left ? "LT" : "RT";
    }

    public enum ControllerType
    {
        Xbox,
        PlayStation,
        Offbrand
    }

    public ControllerType GetCurrentControllerType()
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad == null)
            return ControllerType.Xbox;

        string deviceName = gamepad.name.ToLower();
        string displayName = gamepad.layout.ToLower();

        bool isXbox = deviceName.Contains("xbox") || displayName.Contains("xbox");
        bool isPlayStation = deviceName.Contains("playstation") || displayName.Contains("playstation");
        bool isOffbrand = !isXbox && !isPlayStation;


        if (isXbox)
            return ControllerType.Xbox;

        if (isPlayStation)
            return ControllerType.PlayStation;

        return ControllerType.Offbrand;
    }
}

