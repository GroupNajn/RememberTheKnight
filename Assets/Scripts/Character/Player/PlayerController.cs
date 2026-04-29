using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IKnockbackable
{
    // Made by Jonathan Blixt - edited by Michaëla for stamina

    #region Class Variables
    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] public GameObject _playerCamera;
    PlayerStats playerStats;
    PlayerCombatManager playerCombatManager;
    PlayerManager playerManager;

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

    private bool animCancelable
    {
        get { return playerCombatManager.animationCanceleble; }
        set { }
    }

    public float MovingThreshold = 0.01f;

    public float AnimatorSmoothing = 5f;

    private int attackHash = Animator.StringToHash("Attacking");
    private int dodgeHash = Animator.StringToHash("Dodgeing");
    private int knockbackHash = Animator.StringToHash("Knockback");

    public bool AttackCharged = false;

    [Header("Dodge")]
    public float dodgeAcceleration = 1f;
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
    public bool isKnockedback { get; private set; } = false;
    public Vector3 knockbackForce = Vector3.zero;
    private bool ExplotionInfront;
    #endregion


    private void Start()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotion>();
        PlayerAnimator = GetComponent<Animator>();
        playerState = GetComponent<PlayerStates>();
        playerCombatManager = PlayerCombatManager.Instance;
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerLockRotation = GetComponent<PlayerLockRotation>();

    }

    private void Update()
    {
        if (playerManager.isDead)
        {
            if (knockbackForce != Vector3.zero)
            {
                knockbackForce = Vector3.zero;
                PlayerAnimator.SetBool("IsKnockedbacked", false);
                PlayerAnimator.Play("revive", 0, 0f);
            }
            playerLockRotation.RotationEnabled = false;
            lockHandler.IsLockedOn = false;
            return;
        }

        InitialChecksAndHandlers();
        bool isIdling = playerState.CurrentMoveState == MoveState.Idling;
        bool isDodging = playerState.CurrentMoveState == MoveState.Dodging;
        bool isSprinting = playerState.CurrentMoveState == MoveState.Sprinting;
        bool isLockedOnAndWalking = lockHandler.IsLockedOn && playerState.CurrentMoveState == MoveState.Walking;
        playerLockRotation.RotationEnabled = lockHandler.IsLockedOn && !isSprinting && !isDodging;
        HandleAnimationInputs(isIdling);
        HandleDodge(isDodging);
        HandleAttack();
    }

    private void LateUpdate()
    {
        PlayerAnimator.ResetTrigger("Dodge");
        PlayerAnimator.ResetTrigger("BackStep");
        PlayerAnimator.ResetTrigger("LightAttack");
        PlayerAnimator.ResetTrigger("HeavyAttack");

        if (playerState.CurrentMoveState != MoveState.Knockedback) // if not knockedBack, allow normal movement
            HandleLateralMovement();
    }

    private void InitialChecksAndHandlers()
    {
        HandleKnockback();
        GroundedCheck();
        HandleVerticalMovement();
        CalculateInputMagnitude();
    }

    private void HandleDodge(bool isDodging)
    {
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        bool isKnockedBack = stateInfo.tagHash == knockbackHash || PlayerAnimator.IsInTransition(0) && playerState.CurrentMoveState == MoveState.Knockedback;

        if (playerLocomotionInput.DodgePressed && dodgeCoolDownRemaining <= 0 && animCancelable && !isKnockedBack)
        {
            if (playerStats.currentStamina <= 0)
                return;
            if (PlayerAnimator.GetFloat("Y") <= 0 && math.abs(PlayerAnimator.GetFloat("X")) <= 0.47)
            {
                PlayerAnimator.SetTrigger("BackStep");
                playerState.SetMoveState(MoveState.Dodging);
            }
            else
            {
                PlayerAnimator.SetTrigger("Dodge");
                playerState.SetMoveState(MoveState.Dodging);
            }

            if (playerState.CurrentMoveState == MoveState.Dodging)
            {
                dodgeCoolDownRemaining = playerStats.dodgeCoolDown;

                playerCombatManager.SetStaminaState(StaminaAction.Dodge);
                playerCombatManager.DrainStamina();

            }
            playerCombatManager.SetAnimationCancelebleFalse();
            playerCombatManager.SetStaminaState(StaminaAction.Dodge);
        }

        if (dodgeCoolDownRemaining > 0)
            dodgeCoolDownRemaining -= Time.deltaTime;


        if (isDodging)
            Dodge();
    }

    private void Dodge()
    {
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        Vector3 movementDelta = dodgeDirection * dodgeAcceleration;
        Vector3 newVelocity = _characterController.velocity + movementDelta;

        newVelocity = Vector3.ClampMagnitude(newVelocity, playerStats.currentDodgeSpeedModifier);
        newVelocity.y = _verticalVelocity;

        // un comment for frontflip
        _characterController.Move(transform.rotation.eulerAngles.normalized * playerStats.currentDodgeSpeedModifier * Time.deltaTime);
        _characterController.Move(dodgeDirection * playerStats.currentDodgeSpeedModifier * Time.deltaTime);

        if (!PlayerAnimator.IsInTransition(0) && stateInfo.tagHash != dodgeHash)
        {
      
            playerState.SetMoveState(MoveState.Idling);
            PlayerAnimator.ResetTrigger("Dodge");
            PlayerAnimator.ResetTrigger("BackStep");
            playerCombatManager.SetAnimationCancelebleTrue();
        }
    }

    private void HandleAnimationInputs(bool isIdling)
    {
        if (!lockHandler.IsLockedOn || playerState.CurrentMoveState == MoveState.Sprinting || lockHandler.IsLockedOn && isIdling)
        {
            PlayerAnimator.SetFloat("Y", currentInputMagnitude);
            PlayerAnimator.SetFloat("X", 0);
        }
        else if (lockHandler.IsLockedOn && playerState.CurrentMoveState != MoveState.Sprinting && playerState.CurrentMoveState != MoveState.Dodging)
        {
            PlayerAnimator.SetFloat("Y", currentInputMagnitudeY);
            PlayerAnimator.SetFloat("X", currentInputMagnitudeX);
        }
        else if (isIdling)
        {
            PlayerAnimator.SetFloat("Y", currentInputMagnitude);
            PlayerAnimator.SetFloat("X", 0);
        }
        RotatePlayerToTarget();
    }
   
    private void HandleAttack()
    {
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (playerState.CurrentMoveState == MoveState.Attacking && !PlayerAnimator.IsInTransition(0) && stateInfo.tagHash != attackHash)
        {
            playerState.SetMoveState(MoveState.Idling);
            playerCombatManager.SetAnimationCancelebleTrue();
        }

        if (playerCombatManager.canCharge && playerCombatManager.fullyCharged) // if you can charge attack and you are fully charged, do a charge attack
        {
            PlayerAnimator.SetBool("IsCharged", true);
            PlayerAnimator.SetTrigger("ChargeAttack");
            playerCombatManager.DisableCanCharge();
            playerCombatManager.FullyChargedFalse();
            AttackCharged = true;
            playerState.SetMoveState(MoveState.Attacking);
        }
        else if (playerCombatManager.canCharge && !playerLocomotionInput.AttackCharging) // if you can charge attack, and you are not charging, but you can charge, do a charge attack but not fully charged
        {
            PlayerAnimator.SetBool("IsCharged", false);
            PlayerAnimator.SetTrigger("ChargeAttack");
            playerCombatManager.DisableCanCharge();
            playerCombatManager.FullyChargedFalse();

            playerState.SetMoveState(MoveState.Attacking);
        }
        else if (playerLocomotionInput.AttackPressed && playerStats.currentStamina > 0) // if you can attack 
        {
            if (playerCombatManager.canCombo == true) // if you can combo, do a combo attack
            {
                playerCombatManager.canCombo = false;
                PlayerAnimator.SetTrigger("IsCombo");
                playerState.SetMoveState(MoveState.Attacking);
            }
            else if (animCancelable) // otherwise if you can do a normal attack, do a normal attack
            {
                playerCombatManager.canCombo = false;
                PlayerAnimator.ResetTrigger("IsCombo");
                playerState.SetMoveState(MoveState.Attacking);
                PlayerAnimator.SetTrigger("LightAttack");
            }
        }

        if (playerLocomotionInput.HeavyAttackPressed && playerStats.currentStamina > 0) // if you can attack 
        {
            if (animCancelable || playerCombatManager.canCombo == true) // otherwise if you can do a normal attack, do a normal attack    
            {
                playerState.SetMoveState(MoveState.Attacking);
                PlayerAnimator.SetTrigger("HeavyAttack");
            }

        }

    }

    private void HandleKnockback()
    {
        if (knockbackForce.magnitude > 0.1)
        {
            if (playerState.CurrentMoveState != MoveState.Knockedback)
            {
                playerState.SetMoveState(MoveState.Knockedback);
                playerCombatManager.ResetValues();
                isKnockedback = true;
            }

            _characterController.Move(knockbackForce * Time.deltaTime);
            knockbackForce = Vector3.Lerp(knockbackForce, Vector3.zero, Time.deltaTime * playerStats.currentKnockbackResistance);
        }
        else if (knockbackForce.magnitude <= 0.1 && isKnockedback)
        {
            isKnockedback = false;
            knockbackForce = Vector3.zero;
            
            playerState.SetMoveState(MoveState.Idling);
        }
        PlayerAnimator.SetBool("IsKnockedbacked", playerState.CurrentMoveState == MoveState.Knockedback);
    }

    private void CalculateInputMagnitude()
    {
        bool isDodgeing = playerState.CurrentMoveState == MoveState.Dodging;
        bool isIdling = playerState.CurrentMoveState == MoveState.Idling;
        //==========================X + Y=========================

        float targetMagnitude = playerState.CurrentMoveState == MoveState.Sprinting ? 2f : 1f;
        if (playerState.CurrentMoveState == MoveState.Walking && !lockHandler.IsLockedOn)
            targetMagnitude = 1.5f;

        if (isIdling)
        {
            targetMagnitude = 0f;
        }
        currentInputMagnitude = Mathf.MoveTowards(currentInputMagnitude, targetMagnitude, Time.deltaTime * AnimatorSmoothing);

        //==========================X=========================
        float targetMagnitudeX = playerLocomotionInput.MovementInput.x;

        if (isIdling && playerLocomotionInput.MovementInput.x == 0)
        {
            targetMagnitudeX = 0f;
        }
        currentInputMagnitudeX = Mathf.MoveTowards(currentInputMagnitudeX, targetMagnitudeX, Time.deltaTime * AnimatorSmoothing);

        //==========================Y=========================
        float targetMagnitudeY = playerLocomotionInput.MovementInput.y;

        if (isIdling && playerLocomotionInput.MovementInput.y == 0)
        {
            targetMagnitudeY = 0f;
        }
        currentInputMagnitudeY = Mathf.MoveTowards(currentInputMagnitudeY, targetMagnitudeY, Time.deltaTime * AnimatorSmoothing);
        //Debug.Log($"Magnitude: {currentInputMagnitude}, MagnitudeX: {currentInputMagnitudeX}, MagnitudeY: {currentInputMagnitudeY}");
    }

    private void HandleLateralMovement() //(Horizontal)
    {

        bool isSprinting = playerState.CurrentMoveState == MoveState.Sprinting;

        float lateralAcceleration = isSprinting ? sprintAcceleration : walkAcceleration;
        float clampedLateralMagnitude = isSprinting ? playerStats.currentSprintSpeedModifier : playerStats.currentWalkSpeedModifier;

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

        
        if (animCancelable && !playerState.InActionState())
        {
            bool wantsToSprint = playerLocomotionInput.SprintToggledOn;

            MoveState targetState;

            if (playerLocomotionInput.MovementInput.magnitude <= MovingThreshold)
            {
                targetState = MoveState.Idling;
                playerCombatManager.RegenerateStamina();
            }
            else if (wantsToSprint && playerStats.currentStamina >= 0)
            {
                playerCombatManager.SetStaminaState(StaminaAction.Sprint);
                targetState = MoveState.Sprinting;

                playerCombatManager.DrainStamina();
            }
            else
            {
                targetState = MoveState.Walking;
                playerCombatManager.RegenerateStamina();
            }

            playerState.SetMoveState(targetState);
        }
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
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        bool isKnockedBack = stateInfo.tagHash == knockbackHash || PlayerAnimator.IsInTransition(0) && playerState.CurrentMoveState == MoveState.Knockedback;

        if (playerState.CurrentMoveState != MoveState.Dodging && !isKnockedBack && !playerManager.isDead)
        {
            if (inputDir != Vector2.zero && !lockHandler.IsLockedOn || playerState.CurrentMoveState == MoveState.Sprinting) // calculates rotation for player depending input (8D movement)
            {
                float cameraY = _playerCamera.transform.eulerAngles.y;

                float movementAngle = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

                Quaternion targetRotation = Quaternion.Euler(0f, cameraY + movementAngle, 0f);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
            }
        }

    }

    public void ApplyKnockback(float force, float radius, Vector3 pos)
    {
        float calculatedForce = (force * 3) * (1 - Vector3.Distance(knockbackCalculationPos.position, pos) / radius);
        Vector3 knockbackDirection = (knockbackCalculationPos.position - pos).normalized;
        ExplotionInfront = Vector3.Dot(knockbackDirection, transform.forward.normalized) > 0;
        PlayerAnimator.SetBool("ExplotionInfront", ExplotionInfront);
        knockbackForce = knockbackDirection * calculatedForce;
        //Debug.Log($"Applied  calculated knockbackforce {knockbackForce.magnitude}");

        if (knockbackForce.magnitude < playerStats.currentKnockbackResistance)
        {
            knockbackForce = Vector3.zero;
        }
        //Debug.Log($"Applied knockback with force {knockbackForce.magnitude} Current resitance: {playerStats.currentKnockbackResistance}");
    }

    private void GroundedCheck()
    {
        playerState.IsGrounded = _characterController.isGrounded;
    }


    // CAMERA FIND
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (_playerCamera == null)
        {
            _playerCamera = Camera.main.gameObject;
        }

        if (lockHandler == null)
        {
            lockHandler = FindFirstObjectByType<TargetLockHandler>();
        }
    }
}