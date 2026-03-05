using UnityEngine;
public class PlayerController : MonoBehaviour
{
    #region Class Variables
    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameObject _playerCamera;

    [Header("Movement Settings")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 6f;

    public float sprintAcceleration = 0.5f;
    public float sprintSpeed = 9f;

    public float drag = 0.1f;
    public float gravity = 25f;
    public float playerModelRotationSpeed = 10f;

    public float movingThreshold = 0.01f;

    private float _verticalVelocity = 0f;

    public Animator PlayerAnimator;
    private PlayerLocomotion _playerLocomotionInput;
    #endregion

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    { 
        HandleVerticalMovement();
        HandleLateralMovement();

        // player rotation
        bool isIdling = _playerLocomotionInput.MovementInput.magnitude < movingThreshold;
        PlayerAnimator.SetFloat("Y", 0);
        if (!isIdling)
        {
            RotatePlayerToTarget();
            PlayerAnimator.SetFloat("Y", 1);
        }
    }

    private void HandleLateralMovement()
    {
        bool isGrounded = IsGrounded();

        float lateralAcceleration = _playerLocomotionInput.SprintToggledOn ? runAcceleration : sprintAcceleration;
        float clampedLateralMagnitude = _playerLocomotionInput.SprintToggledOn ? runSpeed : sprintSpeed;

        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraForwardXZ * _playerLocomotionInput.MovementInput.y + cameraRightXZ * _playerLocomotionInput.MovementInput.x;

        Vector3 movementDelta = movementDirection * lateralAcceleration;
        Vector3 newVelocity = _characterController.velocity + movementDelta;

        // Apply drag
        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > currentDrag.magnitude) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(newVelocity, clampedLateralMagnitude);
        newVelocity.y = _verticalVelocity;

        _characterController.Move(newVelocity * Time.deltaTime);
    }

    private void HandleVerticalMovement()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = 0f;
        }
        _verticalVelocity -= gravity * Time.deltaTime;
    }


    private void RotatePlayerToTarget()
    {
        Vector2 movementDir = _playerLocomotionInput.MovementInput;

        if (movementDir != Vector2.zero)
        {
            float cameraY = _playerCamera.transform.eulerAngles.y;

            float movementAngle = Mathf.Atan2(movementDir.x, movementDir.y) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0f, cameraY + movementAngle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerModelRotationSpeed * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        return _characterController.isGrounded;
    }
}