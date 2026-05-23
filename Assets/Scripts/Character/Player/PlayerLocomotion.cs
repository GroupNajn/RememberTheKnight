using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotion : MonoBehaviour
{
    //Made by Jonathan Blixt

    #region Class Variables
    //============= Movement ==================
    [Header("Movement")]

    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool SprintToggledOn = false;
    private bool ignordeNextInput = true; // used for ignoring every second input when using gamepad for sprint toggle


    //============= Combat =============
    public bool AttackPressed { get; private set; }
    public bool AttackCharging = false;
    public bool HeavyAttackPressed { get; private set; }
    public bool HeavyAttackCharging = false;
    #endregion
    private void LateUpdate()
    {
        //movment
        DodgePressed = false;
        //combat
        AttackPressed = false;
        HeavyAttackPressed = false;
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
        bool usingGamepad = InputManager.Instance.usingGamepad;

        if (usingGamepad) // if gamepad
        {
            ignordeNextInput = !ignordeNextInput; 
            if (ignordeNextInput) // if true, ignore this input and wait for the next one to toggle sprint
                return;
            SprintToggledOn = !SprintToggledOn;
        }
        else
            SprintToggledOn = action.isPressed;
    }

    //================ Combat ================

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
            AttackPressed = true;

        AttackCharging = value.isPressed;
    }
    public void OnHeavyAttack(InputValue value)
    {
        if (value.isPressed)
            HeavyAttackPressed = true;

        AttackCharging = value.isPressed;
        HeavyAttackCharging = value.isPressed;
    }
}