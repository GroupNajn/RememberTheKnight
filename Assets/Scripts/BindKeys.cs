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
    public bool keyboardMouseBinding;
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

        if (InputManager.Instance != null && InputManager.Instance.inputActions != null)
        {
            action = InputManager.Instance.inputActions.FindAction(actionReference.action.id);
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
                invalidBinding =
                    !newBinding.StartsWith("<Keyboard>") &&
                    !newBinding.StartsWith("<Mouse>");
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
                InputManager.Instance.SaveBindings();
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

        bindingText.text = action.GetBindingDisplayString(bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
    }
}