using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotion : MonoBehaviour
{
    //Made by Jonathan Blixt

    #region Class Variables
    //============= movement ==================
    [Header("Movement")]
    private PlayerInput PlayerControls;

    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool SprintToggledOn = false;

    //============= action =============
    public bool PickUpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool AttackCharging = false;



    //============= camera =============
    //public Vector2 ScrollInput { get; private set; }

    //[SerializeField] private Camera _virtualCamera;
    //[SerializeField] private float _cameraZoomSpeed = 0.1f;
    //[SerializeField] private float _cameraMinZoom = 1f;
    //[SerializeField] private float _cameraMaxZoom = 5f;
    #endregion

    private void Awake()
    {
        PlayerControls = GetComponent<PlayerInput>();
    }
    private void LateUpdate()
    {
        //movment
        DodgePressed = false;
        AttackPressed = false;

        //camera
        //ScrollInput = Vector2.zero;
    }

    //================ Movement ================
    public void OnDodge(InputValue context)
    {
        DodgePressed = true;
    }

    public void OnLook(InputValue context)
    {
        LookInput = context.Get<Vector2>();
    }

    public void OnMovement(InputValue context)
    {
        MovementInput = context.Get<Vector2>();
    }


    public void OnToggleSprint(InputValue action)
    { 
          SprintToggledOn = action.isPressed;
    }

    //================ Camera ================
    //public void OnScrollCamera(InputValue context)
    //{
    //if (!context.performed)
    //    return;
    // return;
    //Vector2 scrollInput = context.ReadValue<Vector2>();
    //ScrollInput = -1f * scrollInput.normalized * _cameraZoomSpeed;
    // }

    //================ Actions ================
    public void OnPickUp(InputValue context)
    {
        PickUpPressed = true;
    }
    //public void OnAttacking(InputAction.CallbackContext context)
    //{

    //    //Debug.Log("input kom");

    //    if (context.started)
    //    {
    //        AttackPressed = true;   // button pressed
    //    }
    //    else if (context.canceled)
    //    {
    //        AttackPressed = false;  // button released
    //    }
    //}

    public void OnAttacking(InputValue value)
    {
        if (value.isPressed)
            AttackPressed = true;

        AttackCharging = value.isPressed;
        //Debug.Log("ATTACK VALUE: " + value.isPressed);
    }
    void Update()
    {
        if (AttackPressed)
        {
            //Debug.Log("Holding attack");
        }
    }
}