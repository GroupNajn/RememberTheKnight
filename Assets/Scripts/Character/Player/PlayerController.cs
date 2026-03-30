using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;



public class PlayerController : MonoBehaviour, IKnockbackable
{

    // Made by Jonathan Blixt

    #region Class Variables
    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameObject _playerCamera;
    PlayerStats playerStats;
    PlayerCombatManager playerCombatManager;

    [Header("Movement Settings")]
    public float walkAcceleration = 0.25f;
    public float sprintAcceleration = 0.5f;
    public float drag = 0.1f;
    private float currentRotationSpeed
    {

        get
        {
            if (lockHandler.IsLockedOn && playerState.CurrentMoveState == MoveState.Attacking)
                return 0;
            else
                return playerCombatManager.isAttackRotationSpeed ? playerStats.attackRotationSpeed : playerStats.normalRotationSpeed;
        }
        set { playerCombatManager.isAttackRotationSpeed = value == playerStats.attackRotationSpeed; }
    }

    private bool animCanceleble
    {
        get { return playerCombatManager.animationCanceleble; }
        set { }
    }


    public float MovingThreshold = 0.01f;

    public float AnimatorSmoothing = 5f;

    private int attackHash = Animator.StringToHash("Attacking");


    [Header("Dodge")]
    public float dodgeAcceleration = 1f;
    private float dodgeDurationRemaining;
    private float dodgeCoolDownRemaining;
    private Vector3 dodgeDirection;
    public float dodgeDelay = 0.1f;

    private float currentInputMagnitude = 0;
    private float currentInputMagnitudeX = 0;
    private float currentInputMagnitudeY = 0;

    private float _verticalVelocity = 0f;

    private Animator PlayerAnimator;
    private PlayerLocomotion playerLocomotionInput;
    private PlayerStates playerState;
    [SerializeField]
    private TargetLockHandler lockHandler;
    private PlayerLockRotation playerLockRotation;

    [Header("Knockback")]
    public Transform knockbackCalculationPos;
    private bool isKnockedback = false;
    public Vector3 knockbackForce = Vector3.zero;
    #endregion

    private void Start()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotion>();
        PlayerAnimator = GetComponent<Animator>();
        playerState = GetComponent<PlayerStates>();
        playerCombatManager = PlayerCombatManager.Instance;
        playerStats = GetComponent<PlayerStats>();
        playerLockRotation = GetComponent<PlayerLockRotation>();
    }


    private void Update()
    {
        if (playerStats.isDead)
            return;

        InitialChecksAndHandlers();
        bool isIdling = playerState.CurrentMoveState == MoveState.Idling;
        bool isDodging = playerState.CurrentMoveState == MoveState.Dodging;
        bool isLockedOnAndWalking = lockHandler.IsLockedOn && playerState.CurrentMoveState == MoveState.Walking;
        HandleAnimationInputs(isIdling, isLockedOnAndWalking);
        HandleDodge(isIdling, isDodging, isLockedOnAndWalking);
        HandleAttack();
    }

    private void LateUpdate()
    {
        PlayerAnimator.ResetTrigger("Dodge");
        PlayerAnimator.ResetTrigger("LightAttack");
    }

    private void InitialChecksAndHandlers()
    {
        HandleKnockback();
        GroundedCheck();
        HandleVerticalMovement();
        CalculateInputMagnitude();

        if (playerState.CurrentMoveState != MoveState.Dodging && playerState.CurrentMoveState != MoveState.Knockedback) // if not dodging and not knockedBack, allow normal movement
            HandleLateralMovement();
    }

    private void HandleDodge(bool isIdling, bool isDodging, bool isLockedOnAndWalking)
    {

        if(playerLocomotionInput.DodgePressed && dodgeCoolDownRemaining <= 0 && animCanceleble)
        {
            if (lockHandler.IsLockedOn)
            {
                PlayerAnimator.SetTrigger("Dodge");
                playerState.SetMoveState(MoveState.Dodging);
            }
            else if (!isIdling)
            {
                PlayerAnimator.SetTrigger("Dodge");
                playerState.SetMoveState(MoveState.Dodging);
            }
            else if(isIdling)
            {
                PlayerAnimator.SetTrigger("BackStep");
                playerState.SetMoveState(MoveState.Dodging);
            }
            if (playerState.CurrentMoveState == MoveState.Dodging)
            {
                dodgeDurationRemaining = playerStats.dodgeDuration;
                dodgeCoolDownRemaining = playerStats.dodgeCoolDown;
            }
        }

        if (dodgeCoolDownRemaining > 0)
            dodgeCoolDownRemaining -= Time.deltaTime;

        if (isDodging)
            Dodge();
    }

    private void Dodge()
    {
        if (dodgeDurationRemaining == playerStats.dodgeDuration)
        {
            Quaternion playerRotation = Quaternion.Euler(0, transform.rotation.y, 0);
            dodgeDirection = playerRotation * transform.forward;
        }
        dodgeDurationRemaining -= Time.deltaTime;


        if (dodgeDurationRemaining <= playerStats.dodgeDuration - dodgeDelay)
        {
            Vector3 movementDelta = dodgeDirection * dodgeAcceleration;
            Vector3 newVelocity = _characterController.velocity + movementDelta;

            newVelocity = Vector3.ClampMagnitude(newVelocity, playerStats.dodgeSpeedMultiplier);
            newVelocity.y = _verticalVelocity;

            // un comment for frontflip MLG XD
            _characterController.Move(transform.rotation.eulerAngles.normalized * playerStats.dodgeSpeedMultiplier * Time.deltaTime);
            _characterController.Move(dodgeDirection * playerStats.dodgeSpeedMultiplier * Time.deltaTime);
        }
        if (dodgeDurationRemaining <= 0)
        {
            playerState.SetMoveState(MoveState.Idling);
            PlayerAnimator.ResetTrigger("Dodge");
            PlayerAnimator.ResetTrigger("BackStep");
        }
    }

    private void HandleAnimationInputs(bool isIdling, bool isLockedOnAndWalking)
    {
        playerLockRotation.RotationEnabled = isLockedOnAndWalking;
        PlayerAnimator.SetBool("IsLockedOn", lockHandler.IsLockedOn);

        if(playerState.CurrentMoveState != MoveState.Dodging)
        {
            if (!lockHandler.IsLockedOn || playerState.CurrentMoveState == MoveState.Sprinting)
            {
                PlayerAnimator.SetFloat("Y", currentInputMagnitude);
                PlayerAnimator.SetFloat("X", 0);
                RotatePlayerToTarget();
            }
            else if (lockHandler.IsLockedOn)
            {
                PlayerAnimator.SetFloat("Y", currentInputMagnitudeY);
                PlayerAnimator.SetFloat("X", currentInputMagnitudeX);
            }
            else if (isIdling)
            {
                PlayerAnimator.SetFloat("Y", currentInputMagnitude);
                PlayerAnimator.SetFloat("X", 0);
            }
        }
       
    }

    private void HandleAttack()
    {
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (playerState.CurrentMoveState == MoveState.Attacking && !PlayerAnimator.IsInTransition(0) && stateInfo.tagHash != attackHash)
        {
            playerState.SetMoveState(MoveState.Idling);
        }

        if (playerLocomotionInput.AttackPressed && !playerState.InActionState())
        {
            playerState.SetMoveState(MoveState.Attacking);
            PlayerAnimator.SetTrigger("LightAttack");
        }
        else if (playerLocomotionInput.AttackPressed && playerState.CurrentMoveState == MoveState.Attacking && playerCombatManager.canCombo == true)
        {
            PlayerAnimator.SetTrigger("IsCombo");
            playerCombatManager.canCombo = false;
        }
    }

    private void HandleKnockback()
    {
        if (knockbackForce.magnitude > 0.1)
        {
            if (playerState.CurrentMoveState != MoveState.Knockedback)
            {
                playerState.SetMoveState(MoveState.Knockedback);
                isKnockedback = true;
            }

            _characterController.Move(knockbackForce * Time.deltaTime);
            knockbackForce = Vector3.Lerp(knockbackForce, Vector3.zero, Time.deltaTime * playerStats.knockbackResistance);
        }
        else if (knockbackForce.magnitude <= 0.1 && isKnockedback)
        {
            isKnockedback = false;
            knockbackForce = Vector3.zero;
            playerState.SetMoveState(MoveState.Idling);
        }
    }

    private void CalculateInputMagnitude()
    {
        //==========================X + Y=========================

        float targetMagnitude = playerState.CurrentMoveState == MoveState.Sprinting ? 2f : 1f;

        if (playerState.CurrentMoveState == MoveState.Idling)
        {
            targetMagnitude = 0f;
        }
        currentInputMagnitude = Mathf.MoveTowards(currentInputMagnitude, targetMagnitude, Time.deltaTime * AnimatorSmoothing);

        //==========================X=========================
        float targetMagnitudeX = playerLocomotionInput.MovementInput.x;

        if (playerState.CurrentMoveState == MoveState.Idling && playerLocomotionInput.MovementInput.x == 0)
        {
            targetMagnitudeX = 0f;
        }
        currentInputMagnitudeX = Mathf.MoveTowards(currentInputMagnitudeX, targetMagnitudeX, Time.deltaTime * AnimatorSmoothing);

        //==========================Y=========================
        float targetMagnitudeY = playerLocomotionInput.MovementInput.y;

        if (playerState.CurrentMoveState == MoveState.Idling && playerLocomotionInput.MovementInput.y == 0)
        {
            targetMagnitudeY = 0f;
        }
        currentInputMagnitudeY = Mathf.MoveTowards(currentInputMagnitudeY, targetMagnitudeY, Time.deltaTime * AnimatorSmoothing);
    }

    private void HandleLateralMovement() //(Horizontal)
    {
        float lateralAcceleration = playerLocomotionInput.SprintToggledOn ? sprintAcceleration : walkAcceleration;
        float clampedLateralMagnitude = playerLocomotionInput.SprintToggledOn ? playerStats.sprintSpeedMultiplier : playerStats.walkSpeedMultiplier;

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

        if (!playerState.InActionState()) // if not dodging or attacking, change movement state to change depending on input, walking, sprinting or idling
            playerState.SetMoveState(playerLocomotionInput.MovementInput.magnitude > MovingThreshold ? (playerLocomotionInput.SprintToggledOn ? MoveState.Sprinting : MoveState.Walking) : MoveState.Idling);
    }

    private void HandleVerticalMovement()
    {
        if (playerState.IsGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = 0f;
        }
        _verticalVelocity -= playerStats.gravity * Time.deltaTime;
    }

    private void RotatePlayerToTarget()
    {
        Vector2 inputDir = playerLocomotionInput.MovementInput;

        if (inputDir != Vector2.zero && playerState.CurrentMoveState != MoveState.Dodging) // calculates rotation for player depending input (8D movement)
        {
            float cameraY = _playerCamera.transform.eulerAngles.y;

            float movementAngle = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0f, cameraY + movementAngle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
        }
    }

    public void ApplyKnockback(float force, float radius, Vector3 pos)
    {
        float calculatedForce = force * (1 - Vector3.Distance(knockbackCalculationPos.position, pos) / radius);
        Vector3 knockbackDirection = (knockbackCalculationPos.position - pos).normalized;
        knockbackForce = knockbackDirection * calculatedForce;
    }

    private void GroundedCheck()
    {
        playerState.IsGrounded = _characterController.isGrounded;
    }
}