using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class SliderNavigationManager : MonoBehaviour
{
    [SerializeField] private float StepAmount = 0.1f;

    [SerializeField] private Color selectedColor = Color.gray;
    [SerializeField] private Color editingColor = Color.black;

    private Slider currentSlider;
    private Graphic currentHandleGraphic;
    private Color originalHandleColor;
    private bool isEditing;

    private Navigation originalNavigation;

    private void Update()
    {
        CheckSelectedSlider();

        if (currentSlider == null)
            return;

        Gamepad gamepad = Gamepad.current;

        if (gamepad == null)
            return;

        if (!isEditing && gamepad.buttonSouth.wasPressedThisFrame)
        {
            EnterSliderEditMode();
            return;
        }

        if (isEditing && gamepad.buttonEast.wasPressedThisFrame)
        {
            ExitSliderEditMode();
            return;
        }

        if (!isEditing)
            return;

        if (gamepad.dpad.right.wasPressedThisFrame)
        {
            currentSlider.value += StepAmount;
        }
        else if (gamepad.dpad.left.wasPressedThisFrame)
        {
            currentSlider.value -= StepAmount;
        }
    }
    private void LateUpdate()
    {
        if (currentHandleGraphic == null)
            return;

        if (isEditing)
        {
            ApplyHandleColor(editingColor);
        }
        else if (currentSlider != null)
        {
            ApplyHandleColor(selectedColor);
        }
    }


    private void CheckSelectedSlider()
    {
        if (EventSystem.current == null)
            return;

        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;

        Slider selectedSlider = null;

        if (selectedGameObject != null)
        {
            selectedSlider = selectedGameObject.GetComponent<Slider>();

            if (selectedSlider == null)
                selectedSlider = selectedGameObject.GetComponentInParent<Slider>();
        }

        if (selectedSlider == currentSlider)
            return;

        ResetOldSlider();

        currentSlider = selectedSlider;
        isEditing = false;

        if (currentSlider == null)
            return;

        originalNavigation = currentSlider.navigation;

        if (currentSlider.handleRect != null)
        {
            currentHandleGraphic = currentSlider.handleRect.GetComponent<Graphic>();

            if (currentHandleGraphic == null)
                currentHandleGraphic = currentSlider.handleRect.GetComponentInChildren<Graphic>();
        }

        if (currentHandleGraphic != null)
        {
            originalHandleColor = currentHandleGraphic.color;
            ApplyHandleColor(selectedColor);
        }
    }
    private void EnterSliderEditMode()
    {
        if (currentSlider == null)
            return;

        isEditing = true;

        originalNavigation = currentSlider.navigation;

        Navigation noNavigation = currentSlider.navigation;
        noNavigation.mode = Navigation.Mode.None;
        currentSlider.navigation = noNavigation;

        ApplyHandleColor(editingColor);

        EventSystem.current.SetSelectedGameObject(currentSlider.gameObject);
    }
    private void ExitSliderEditMode()
    {
        if (currentSlider == null)
            return;

        isEditing = false;

        currentSlider.navigation = originalNavigation;

        ApplyHandleColor(selectedColor);

        EventSystem.current.SetSelectedGameObject(currentSlider.gameObject);
    }

    private void ResetOldSlider()
    {
        if (currentSlider != null && isEditing)
        {
            currentSlider.navigation = originalNavigation;
        }

        if (currentHandleGraphic != null)
        {
            ApplyHandleColor(originalHandleColor);
        }

        currentSlider = null;
        currentHandleGraphic = null;
        isEditing = false;
    }

    private void ApplyHandleColor(Color color)
    {
        if (currentHandleGraphic == null)
            return;

        currentHandleGraphic.color = color;
        currentHandleGraphic.canvasRenderer.SetColor(color);
    }
}