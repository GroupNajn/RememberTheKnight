using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
public class PlayerController : MonoBehaviour
{

    // Made by Jonathan Blixt

    #region Class Variables
    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameObject _playerCamera;

    [Header("Movement Settings")]
    public float walkAcceleration = 0.25f;
    public float walkSpeedMultiplier = 0f;

    public float sprintAcceleration = 0.5f;
    public float sprintSpeedMultiplier = 0f;

    public float drag = 0.1f;
    public float gravity = 25f;
    public float playerModelRotationSpeed = 10f;

    public float movingThreshold = 0.01f;

    public float AnimatorSmoothing = 5f;


    [Header("Dodge")]
    public float dodgeDuration = 0.2f;
    public float dodgeSpeedMultiplier = 0f;
    public float dodgeAcceleration = 1f;
    private float dodgeDurationRemaining;
    public float dodgeCoolDown = 1f;
    private float dodgeCoolDownRemaining;
    private Vector3 dodgeDirection;
    public float dodgeDelay = 0.1f;

    private float currentInputMagnitude = 0;


    private float _verticalVelocity = 0f;

    private Animator PlayerAnimator;
    private PlayerLocomotion playerLocomotionInput;
    private PlayerStates playerState;


    //private bool isDodging = false;
    #endregion

    private void Awake()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotion>();
        PlayerAnimator = GetComponent<Animator>();
        playerState = GetComponent<PlayerStates>();
    }

    private void Update()
    {
        //Grounded?
        GroundedCheck();

        HandleVerticalMovement();

        if(!playerState.InActionState())
        CalculateInputMagnitude();

        if (playerState.CurrentMoveState != MoveState.Dodging) // if not dodging, allow normal movement
            HandleLateralMovement();

        //Idling?
        if(playerLocomotionInput.MovementInput.magnitude < movingThreshold && !playerState.InActionState())
        { playerState.SetMoveState(MoveState.Idling);}

        bool isIdling = playerState.CurrentMoveState == MoveState.Idling;

        //dodging?
        bool isDodging = playerState.CurrentMoveState == MoveState.Dodging;
        PlayerAnimator.SetFloat("Y", currentInputMagnitude);

        if (!isIdling)
        {
            RotatePlayerToTarget();

            //Dodgeing
            if (playerLocomotionInput.DodgePressed && dodgeCoolDownRemaining <= 0 && !playerState.InActionState() )
            {
                PlayerAnimator.SetTrigger("Dodge");
                playerState.SetMoveState(MoveState.Dodging);
                dodgeDurationRemaining = dodgeDuration;
                dodgeCoolDownRemaining = dodgeCoolDown;
                Dodge();
            }
        }

        if (dodgeCoolDownRemaining > 0)
        {
            dodgeCoolDownRemaining -= Time.deltaTime;
        }

        if (isDodging)
        {
            Dodge();
        }
       
        int AttackHash = Animator.StringToHash("Attacking");

        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (playerState.CurrentMoveState == MoveState.Attacking && !PlayerAnimator.IsInTransition(0)  && stateInfo.tagHash != AttackHash)
        {
            Debug.Log("Attack animation finished, returning to idling");
            playerState.SetMoveState(MoveState.Idling);
        }

        if (playerLocomotionInput.AttackPressed && !playerState.InActionState())
        {
            playerState.SetMoveState(MoveState.Attacking);
            PlayerAnimator.SetTrigger("LightAttack");
        }
    }

    private void LateUpdate()
    {
        PlayerAnimator.ResetTrigger("Dodge");
        PlayerAnimator.ResetTrigger("LightAttack");
    }

    private void CalculateInputMagnitude()
    {
        float targetMagnitude = playerState.CurrentMoveState == MoveState.Sprinting ? 2f : 1f;

        if(playerState.CurrentMoveState == MoveState.Idling)
        {
            targetMagnitude = 0f;
        }
        currentInputMagnitude = Mathf.MoveTowards(currentInputMagnitude, targetMagnitude, Time.deltaTime * AnimatorSmoothing);
    }

    private void Dodge()
    {
        if (dodgeDurationRemaining == dodgeDuration)
        {
            Quaternion playerRotation = Quaternion.Euler(0, transform.rotation.y, 0);
            dodgeDirection = playerRotation * transform.forward;
        }
        dodgeDurationRemaining -= Time.deltaTime;


        if (dodgeDurationRemaining <= dodgeDuration - dodgeDelay)
        {
            Vector3 movementDelta = dodgeDirection * dodgeAcceleration;
            Vector3 newVelocity = _characterController.velocity + movementDelta;

            newVelocity = Vector3.ClampMagnitude(newVelocity, dodgeSpeedMultiplier);
            newVelocity.y = _verticalVelocity;

            // un comment for frontflip MLG XD
            _characterController.Move(transform.rotation.eulerAngles.normalized * dodgeSpeedMultiplier * Time.deltaTime);
            _characterController.Move(dodgeDirection * dodgeSpeedMultiplier * Time.deltaTime);
        }
        if (dodgeDurationRemaining <= 0)
        {
            playerState.SetMoveState(MoveState.Idling);
            PlayerAnimator.ResetTrigger("Dodge");
        }

    }

    private void HandleLateralMovement() //(Horizontal)
    {
        float lateralAcceleration = playerLocomotionInput.SprintToggledOn ? walkAcceleration : sprintAcceleration;
        float clampedLateralMagnitude = playerLocomotionInput.SprintToggledOn ? walkSpeedMultiplier : sprintSpeedMultiplier;

        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraForwardXZ * playerLocomotionInput.MovementInput.y + cameraRightXZ * playerLocomotionInput.MovementInput.x;

        Vector3 movementDelta = movementDirection * lateralAcceleration;
        Vector3 newVelocity = _characterController.velocity + movementDelta;

        // Apply drag
        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > currentDrag.magnitude) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(newVelocity, clampedLateralMagnitude);
        newVelocity.y = _verticalVelocity;

        _characterController.Move(newVelocity * Time.deltaTime);

        if(!playerState.InActionState()) // if not dodging or attacking, change movement state to change depending on input, walking, sprinting or idling
            playerState.SetMoveState(playerLocomotionInput.MovementInput.magnitude > movingThreshold ? (playerLocomotionInput.SprintToggledOn ? MoveState.Walking : MoveState.Sprinting) : MoveState.Idling);
    }

    private void HandleVerticalMovement()
    {

        if (playerState.IsGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = 0f;
        }
        _verticalVelocity -= gravity * Time.deltaTime;
    }


    private void RotatePlayerToTarget()
    {
        Vector2 inputDir = playerLocomotionInput.MovementInput;

        if (inputDir != Vector2.zero && playerState.CurrentMoveState != MoveState.Dodging) // calculates rotation for player depending input (8D movement)
        {
            float cameraY = _playerCamera.transform.eulerAngles.y;

            float movementAngle = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0f, cameraY + movementAngle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerModelRotationSpeed * Time.deltaTime);
        }
    }

    private void GroundedCheck()
    {
        playerState.IsGrounded = _characterController.isGrounded;
      //  return _characterController.isGrounded;
    }
}