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
    BackStepping = 7,
    Knockedback = 8
}

public class PlayerStates : MonoBehaviour
{
    [field: SerializeField] public MoveState CurrentMoveState { get; private set; } = MoveState.Idling;
    GameObject player;
    PlayerController playerController;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    public bool IsGrounded;
    public void SetMoveState(MoveState playerMovementState)
    {
        CurrentMoveState = playerMovementState;
       // playerController.AttackCharged = false;
    }
    public bool InActionState()
    {
        return IsStateActionState(CurrentMoveState);
    }
    public bool IsStateActionState(MoveState movementState) // retun true if the movement state is one of the grounded states, otherwise return false
    {
        return
               movementState == MoveState.Dodging ||
               movementState == MoveState.Dieing ||
               movementState == MoveState.Attacking ||
               movementState == MoveState.BackStepping ||
               movementState == MoveState.Knockedback;
    }
}