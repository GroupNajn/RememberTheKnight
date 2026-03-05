using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotion : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions, PlayerControls.IThirdPersonMapActions , PlayerControls.IPlayerActionsMapActions
{
    #region Class Variables
    //movement
    [Header("Movement")]
    [SerializeField] private bool holdToSprint = true;
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool SprintToggledOn { get; private set; }

    //action
    public bool PickUpPressed { get; private set; }
    public bool AttackPressed { get; private set; }

    //camera
    //public Vector2 ScrollInput { get; private set; }

    //[SerializeField] private Camera _virtualCamera;
    //[SerializeField] private float _cameraZoomSpeed = 0.1f;
    //[SerializeField] private float _cameraMinZoom = 1f;
    //[SerializeField] private float _cameraMaxZoom = 5f;
    #endregion

    private void LateUpdate()
    {
        //movment
        DodgePressed = false;

        //camera
        //ScrollInput = Vector2.zero;
    }

    //================ Movement ================
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        DodgePressed = true;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnToggleSprint(InputAction.CallbackContext context)
    {
        if(context.performed)
            {
            SprintToggledOn = holdToSprint || !SprintToggledOn;
        }
            else if (context.canceled)
        {
            SprintToggledOn = !holdToSprint && SprintToggledOn;
        }
    }
    //================ Camera ================
    public void OnScrollCamera(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        return;
        //Vector2 scrollInput = context.ReadValue<Vector2>();
        //ScrollInput = -1f * scrollInput.normalized * _cameraZoomSpeed;
    }

    //================ Actions ================
    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        PickUpPressed = true;
    }
    public void OnAttacking(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        AttackPressed = true;
    }
}
