using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(NavMeshAgent))]
public class RigidbodyCharacterController : MonoBehaviour
{
    [SerializeField] private string[] blockedMovementStates = { "Idle", "Awake", "Agitate", "Attack", "Leap Attack" };
    [SerializeField] private float minMovementSpeed = 0.1f;
    [SerializeField] private float slopeDownForce = -5f;

    private Animator animator;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator.applyRootMotion = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void OnAnimatorMove()
    {
        // Check if movement should be blocked first
        if (IsMovementBlockedByName())
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        Vector3 deltaPos = animator.deltaPosition;
        Quaternion deltaRot = animator.deltaRotation;

        if (deltaPos.magnitude < minMovementSpeed * Time.deltaTime)
            return;


        if (isGrounded)
            deltaPos.y = slopeDownForce * Time.deltaTime;


        rb.MovePosition(rb.position + deltaPos);
        rb.MoveRotation(rb.rotation * deltaRot);

        agent.ResetPath();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out _, .2f);
        bool isBlocked = IsMovementBlockedByName();
        agent.updatePosition = !isBlocked && isGrounded;
        agent.updateRotation = !isBlocked && isGrounded;
    }

    private bool IsMovementBlockedByName()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        foreach (string name in blockedMovementStates)
        {
            if (stateInfo.IsName(name))
            {
                return true;
            }
        }
        return false;
    }
}
