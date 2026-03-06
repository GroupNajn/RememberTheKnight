using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotion : MonoBehaviour
{

    //Made by Jonathan Blixt

    #region Class Variables
    //============= movement ==================
    [Header("Movement")]
    private PlayerInput PlayerControls;

    [SerializeField] private bool holdToSprint = true;
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool SprintToggledOn { get; private set; }

    //============= action =============
    public bool PickUpPressed { get; private set; }
    public bool AttackPressed { get; private set; }

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

        //camera
        //ScrollInput = Vector2.zero;
    }

    //================ Movement ================
    public void OnDodge(InputValue context)
    {
        //if (!context.performed)
        //    return;

        DodgePressed = true;
    }

    public void OnLook(InputValue context)
    {
        LookInput = context.Get<Vector2>();
    }

    public void OnMovement(InputValue context)
    {
        Debug.Log("Movement input received: " + context.Get<Vector2>());
        MovementInput = context.Get<Vector2>();
    }

    public void OnToggleSprint(InputValue context)
    {
         SprintToggledOn = !SprintToggledOn;
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
        //if (!context.performed)
        //    return;

        PickUpPressed = true;
    }
    public void OnAttacking(InputValue context)
    {
        //if (!context.performed)
        //    return;

        AttackPressed = true;
    }
}