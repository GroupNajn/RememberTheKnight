using UnityEngine;
//Made by Jonathan Blixt
//=========States========
public enum MoveState
{
    Idling = 0,
    Walking = 1,
    Sprinting = 2,
    Dodging = 3,
    Falling = 4,
    Dieing = 5,
    Attacking = 6,
    BackStepping = 7
}

public class PlayerStates : MonoBehaviour
{
    [field: SerializeField] public MoveState CurrentMoveState { get; private set; } = MoveState.Idling;

    public bool IsGrounded;
    public void SetMoveState(MoveState playerMovementState)
    {
        CurrentMoveState = playerMovementState;
    }

    //public bool InGroundedState()
    //{
    //    return IsStateGroundedState(CurrentMoveState);
    //}

    //public bool IsStateGroundedState(MoveState movementState) // retutrue if the movement state is one of the grounded states, otherwise return false
    //{
    //    return movementState == MoveState.Idling ||
    //           movementState == MoveState.Walking ||
    //           movementState == MoveState.Sprinting ||
    //           movementState == MoveState.Dodging ||
    //           movementState == MoveState.Dieing ||
    //           movementState == MoveState.Attacking ||
    //           movementState == MoveState.BackStepping;
    //}
    public bool InActionState()
    {
        return IsStateActionState(CurrentMoveState);
    }
    public bool IsStateActionState(MoveState movementState) // retutrue if the movement state is one of the grounded states, otherwise return false
    {
        return
               movementState == MoveState.Dodging ||
               movementState == MoveState.Dieing ||
               movementState == MoveState.Attacking ||
               movementState == MoveState.BackStepping;
    }
}