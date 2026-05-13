using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class BindKeys : MonoBehaviour
{
    [Header("Action")]
    public InputActionReference actionReference;

    [Header("Binding")]
    public int bindingIndex;

    [Header("UI")]
    public TextMeshProUGUI bindingText;

    [Header("Device Restrictions")]
    public bool keyboardBinding;
    public bool gamepadBinding;

    private InputAction action;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        if (actionReference == null)
        {
            Debug.LogError($"Missing InputActionReference on {gameObject.name}");
            return;
        }

        action = actionReference.action;

        if (bindingText == null)
            bindingText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateBindingDisplay();
    }

    public void StartRebind()
    {
        if (action == null || bindingText == null)
            return;

        action.Disable();

        bindingText.text = "...";

        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Mouse>/scroll")
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Pointer>/delta")
            .WithControlsExcluding("<Gamepad>/leftStick")
            .WithControlsExcluding("<Gamepad>/rightStick")
            .WithCancelingThrough("<Keyboard>/escape");

        if (keyboardBinding)
        {
            rebindingOperation.WithControlsHavingToMatchPath("<Keyboard>");
            rebindingOperation.WithControlsHavingToMatchPath("<Mouse>");
        }


        if (gamepadBinding)
            rebindingOperation.WithControlsHavingToMatchPath("<Gamepad>");

        rebindingOperation.OnComplete(operation =>
        {
            bool duplicate = false;

            var newBinding = action.bindings[bindingIndex].effectivePath;

            foreach (var map in action.actionMap.asset.actionMaps)
            {
                foreach (var otherAction in map.actions)
                {
                    for (int i = 0; i < otherAction.bindings.Count; i++)
                    {
                        if (otherAction == action && i == bindingIndex)
                            continue;

                        if (otherAction.bindings[i].effectivePath == newBinding)
                            duplicate = true;
                    }
                }
            }

            if (duplicate)
                action.RemoveBindingOverride(bindingIndex);

            action.Enable();

            operation.Dispose();

            UpdateBindingDisplay();

            InputManager.Instance.SaveBindings();
        });

        rebindingOperation.Start();
    }

    public void UpdateBindingDisplay()
    {
        if (action == null || bindingText == null)
            return;

        bindingText.text = action.GetBindingDisplayString(bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
    }
}

