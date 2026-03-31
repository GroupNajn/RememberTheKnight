using System.Collections;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    [Header("Bounce")]
    [SerializeField] private float launchSpeed = 3f;
    [SerializeField] private float launchHeight = 2f;
    [SerializeField] private int bounceCount = 3;
    [SerializeField] private float launchDuration = 0.25f;
    [SerializeField] private float landDamping = 0.5f;

    private Vector3 horizontalDirection;
    private float groundY;
    private bool hasLanded = false;
    private Vector3 velocity;
    private Vector3 velocityBeforeElapsedTime;
    public Vector3 Velocity => velocityBeforeElapsedTime;
    public bool HasLanded => hasLanded;

    void Start()
    {
        groundY = transform.position.y;
        horizontalDirection = GetRandomDirection();
        StartCoroutine(LaunchOnSpawn());

    }

 


    private IEnumerator LaunchOnSpawn()
    {
        float currentHeight = launchHeight;
        Vector3 currentPos = transform.position;

        for (int i = 0; i < bounceCount; i++)
        {
            float elapsed = 0f;

            if (velocity == Vector3.zero)
            {
                Vector3 horizontalVelocity = horizontalDirection * launchSpeed;
                velocity = new Vector3(horizontalVelocity.x, 0f, horizontalVelocity.z);
            }

            while (elapsed < launchDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / launchDuration);

                float parabola = 4f * t * (1f - t);
                float yOffset = parabola * currentHeight;

                // Horizontal velocity
                Vector3 horizontalVelocity = horizontalDirection * launchSpeed;

                // Derivatan av 4 * t * (1 - t) är 4 - 8t
                
                float yVelocity = (4f * currentHeight * (1f - 2f * t)) / launchDuration;

                velocity = new Vector3(horizontalVelocity.x, yVelocity, horizontalVelocity.z);

                currentPos += new Vector3(velocity.x, 0f, velocity.z) * Time.deltaTime;

                transform.position = new Vector3(currentPos.x, groundY + yOffset, currentPos.z);

                if (elapsed > launchDuration) velocityBeforeElapsedTime = velocity; 
                

                yield return null;
            }

            currentPos = new Vector3(currentPos.x, groundY, currentPos.z);
            transform.position = currentPos;

            currentHeight *= landDamping;
            launchSpeed *= 0.7f;

            Vector3 newHorizontalVelocity = horizontalDirection * launchSpeed;
            velocity = new Vector3(newHorizontalVelocity.x, 0f, newHorizontalVelocity.z);
        }

        hasLanded = true;
    }

    private Vector3 GetRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 dir = new Vector3(x, 0f, z).normalized;

        if (dir == Vector3.zero)
            dir = Vector3.forward;

        return dir;
    }
}