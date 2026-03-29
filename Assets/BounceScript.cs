using System.Collections;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    [Header("Bounce")]
    [SerializeField] private float launchSpeed = 3f;
    [SerializeField] private float firstBounceHeight = 2f;
    [SerializeField] private int bounceCount = 3;
    [SerializeField] private float bounceDuration = 0.25f;
    [SerializeField] private float bounceDamping = 0.5f;

    private Vector3 horizontalDirection;
    private float groundY;
    private bool finishedBounce = false;

    public bool FinishedBounce => finishedBounce;

    void Start()
    {
        groundY = transform.position.y;
        horizontalDirection = GetRandomDirection();

        StartCoroutine(BounceRoutine());
    }

    private IEnumerator BounceRoutine()
    {
        float currentHeight = firstBounceHeight;
        Vector3 currentPos = transform.position;

        for (int i = 0; i < bounceCount; i++)
        {
            float elapsed = 0f;

            while (elapsed < bounceDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / bounceDuration);

                Debug.Log(t);
                float parabola = 4f * t * (1f - t);
                float yOffset = parabola * currentHeight;

               
                Vector3 horizontalMove = horizontalDirection * launchSpeed * Time.deltaTime;
                currentPos += horizontalMove;

                transform.position = new Vector3(
                    currentPos.x,
                    groundY + yOffset,
                    currentPos.z
                );

                yield return null;
            }

           
            currentPos = new Vector3(transform.position.x, groundY, transform.position.z);
            transform.position = currentPos;

            
            currentHeight *= bounceDamping;
            launchSpeed *= 0.7f;
        }
       
        finishedBounce = true;
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