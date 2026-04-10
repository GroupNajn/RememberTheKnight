using System.Collections;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float bounceDamping = 0.5f;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private LayerMask environmentMask;
    [SerializeField] private float ignorePlayerTime = 1.5f;

    private bool collisionRestored = false;

    private Rigidbody rb;
    private Collider myCollider;
    private int bounceCount = 0;
    private bool hasLanded = false;

    private Vector3 lastBouncePosition;
    public Vector3 LastBouncePosition => lastBouncePosition;
    private Vector3 horizontalDir;

    public bool HasLanded => hasLanded;
    public Vector3 Velocity => rb.linearVelocity;

    private Collider[] playerColliders;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

        IgnorePlayer();

        horizontalDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        Vector3 launchVelocity = horizontalDir * launchForce;
        launchVelocity.y = launchForce;

        rb.linearVelocity = launchVelocity;
        rb.freezeRotation = true;
    }

    private void IgnorePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerColliders = player.GetComponentsInChildren<Collider>();

            foreach (Collider col in playerColliders)
            {
                if (col != null)
                    Physics.IgnoreCollision(myCollider, col, true);
            }
        }
    }
    private void RestorePlayerCollision()
    {
        if (playerColliders != null)
        {
            foreach (Collider col in playerColliders)
            {
                if (col != null)
                    Physics.IgnoreCollision(myCollider, col, false);
            }
        }
    }




    private void Update()
    {
        if(!collisionRestored && hasLanded)
        {
            RestorePlayerCollision();
            collisionRestored = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment")) // Ignore Environement layer
        {
            bounceCount++; 

            if (bounceCount >= maxBounces)
            {
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 5f, environmentMask)) 
                { // Raycast to save position of last landing to use for transition to hover state, needed for smooth lerp into the prefered distance above ground 
                    // where last bounce was. Is used inside of the "loot_Hover script and Loot_Follow" script. 
                    float yOffset = myCollider.bounds.extents.y;
                    lastBouncePosition = hit.point + Vector3.up * yOffset;
                    transform.position = lastBouncePosition;
                }
                else
                {
                    lastBouncePosition = transform.position;
                }
                // Disabling of rigidbody properties for custom gravity and movement. 
                hasLanded = true;

                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
                rb.useGravity = false;
                myCollider.isTrigger = true;

                return;
            }

            float yVel = Mathf.Max(Mathf.Abs(rb.linearVelocity.y) * bounceDamping, 4f); // Dampen the bounce

            float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude; 
            horizontalSpeed *= 0.9f; // Decrease the horizontal speed
            horizontalSpeed = Mathf.Max(horizontalSpeed, 2f); 

            Vector3 newVelocity = horizontalDir * horizontalSpeed;
            newVelocity.y = yVel;

            rb.linearVelocity = newVelocity;
        }
    }
}