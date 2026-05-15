using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IPickupable;

public class BounceScript : MonoBehaviour
{
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float bounceDamping = 0.5f;
    [SerializeField] private int maxBounces = 2;
    [SerializeField] private LayerMask environmentMask;

    [Header("Physics Collider")]
    [SerializeField] private Collider physicsCollider;

    [Header("On Enter Collider")]
    [SerializeField] private Collider pickupCollider;

    public LayerMask EnvironmentMask => environmentMask;

    private bool collisionRestored = false;

    private Rigidbody rb;
    private int bounceCount = 0;
    private bool hasLanded = false;

    private Vector3 lastBouncePosition;
    public Vector3 LastBouncePosition => lastBouncePosition;

    private Vector3 horizontalDir;

    public bool HasLanded => hasLanded;
    public Vector3 Velocity => rb.linearVelocity;

    private Collider[] playerColliders;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (physicsCollider == null)
        {
            Debug.LogError("PhysicsCollider saknas på " + gameObject.name);
            return;
        }

        physicsCollider.isTrigger = false;

        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
            StartCoroutine(EnablePickupColliderAfterDelay(2.5f));
        }

        horizontalDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        Vector3 launchVelocity = horizontalDir * launchForce;
        launchVelocity.y = launchForce;

        rb.linearVelocity = launchVelocity;
        rb.freezeRotation = true;
    }

    private void IgnorePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null) return;


        Transform root = player.transform.root;


        playerColliders = root.GetComponentsInChildren<Collider>(true);

        foreach (Collider col in playerColliders)
        {
            if (col != null && physicsCollider != null)
            {
                Physics.IgnoreCollision(physicsCollider, col, true);
            }
        }
    }

    private void RestorePlayerCollision()
    {
        if (playerColliders == null) return;

        foreach (Collider col in playerColliders)
        {
            if (col != null)
            {
                Physics.IgnoreCollision(physicsCollider, col, false);
            }
        }
    }

    private void Update()
    {
        if (!collisionRestored && hasLanded)
        {
            RestorePlayerCollision();
            collisionRestored = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        if (!IsEnvironmentLayer(collision.gameObject.layer))
            return;

        if (TryGetComponent<Soul>(out Soul soul))
        {
            RuntimeManager.PlayOneShotAttached(gameObject.GetComponent<Soul>().soulBounceEvent, gameObject);
        }

        bounceCount++;

        if (bounceCount >= maxBounces)
        {
            Land();
            return;
        }

        Bounce();
    }

    private bool IsEnvironmentLayer(int layer)
    {
        return collisionLayerIsInMask(layer, environmentMask);
    }

    private bool collisionLayerIsInMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    private void Land()
    {
        Vector3 rayStart = physicsCollider.bounds.center + Vector3.up * 2f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 10f, environmentMask))
        {
            float bottomOffset = physicsCollider.bounds.center.y - physicsCollider.bounds.min.y;
            float safetyOffset = 0.08f;

            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + bottomOffset + safetyOffset;

            float maxSnapDistance = 2f;

            if (Vector3.Distance(transform.position, newPosition) <= maxSnapDistance)
            {
                lastBouncePosition = newPosition;
            }
            else
            {
                lastBouncePosition = transform.position;
            }
        }
        else
        {
            lastBouncePosition = transform.position;
        }

        hasLanded = true;

        //if (pickupCollider != null)
        //    pickupCollider.enabled = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private IEnumerator EnablePickupColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (pickupCollider != null)
        {
            pickupCollider.enabled = true;
        }
    }

    private void Bounce()
    {
        if (this == null) return;
        float yVel = Mathf.Max(Mathf.Abs(rb.linearVelocity.y) * bounceDamping, 4f);

        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

        horizontalSpeed *= 0.9f;
        horizontalSpeed = Mathf.Max(horizontalSpeed, 2f);

        Vector3 newVelocity = horizontalDir * horizontalSpeed;
        newVelocity.y = yVel;

        rb.linearVelocity = newVelocity;
    }
}