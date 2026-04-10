using UnityEngine;

public class Loot_Hover : MonoBehaviour
{
    [SerializeField] float hoverSpeed = 1.0f;
    [SerializeField] float hoverHeight = 1.0f;
    [SerializeField] private float lerpSpeed = 2f;
    [SerializeField] private float arriveThreshold = 0.005f;
    [SerializeField] private float hoverBlendSpeed = 4f;

    private bool isLerping = true;
    private bool isBlendingToHover = false;
    private bool isHovering = false;

    private float hoverOffset;

    private Vector3 basePosition;
    private bool initialized = false;

    private BounceScript bounce;

    void Start()
    {
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);
        bounce = GetComponent<BounceScript>();
    }

    void Update()
    {
        if (bounce == null) return;
        if (!bounce.HasLanded) return;

        if (!initialized)
        {
            basePosition = bounce.LastBouncePosition + Vector3.up * 0.8f;
            initialized = true;
            isLerping = true;
            isBlendingToHover = false;
            isHovering = false;
        }

        if (isLerping)
        {
            transform.position = Vector3.Lerp(transform.position, basePosition, Time.deltaTime * lerpSpeed);

            if (Vector3.Distance(transform.position, basePosition) < arriveThreshold)
            {
                isLerping = false;
                isBlendingToHover = true;
            }

            return;
        }

        if (isBlendingToHover)
        {
            Vector3 hoverTarget = GetHoverPosition();

            transform.position = Vector3.Lerp(transform.position, hoverTarget, Time.deltaTime * hoverBlendSpeed);

            if (Vector3.Distance(transform.position, hoverTarget) < arriveThreshold)
            {
                isBlendingToHover = false;
                isHovering = true;
            }

            return;
        }

        if (isHovering)
        {
            HoverSinWave();
        }
    }

    private Vector3 GetHoverPosition()
    {
        float y = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverHeight * 0.01f;
        return basePosition + new Vector3(0f, y, 0f);
    }

    private void HoverSinWave()
    {
        transform.position = GetHoverPosition();
    }
}
