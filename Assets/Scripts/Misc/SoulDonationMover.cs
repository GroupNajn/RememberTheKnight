using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class DonationSoulMover : MonoBehaviour
{
    private SplineContainer spline;
    private Transform center;

    private float moveDuration;
    private float timer;
    private bool isMoving;

    [SerializeField] private float waveAmplitude = 0.5f;
    [SerializeField] private float waveFrequency = 8f;
    [SerializeField] private float rotationSpeed = 180f;

    public void Initialize(SplineContainer splineContainer, float duration)
    {
        spline = splineContainer;
        
        moveDuration = duration;

        center = FindChildRecursive(transform, "Center");

        if (center == null)
        {
            Debug.LogError($"{gameObject.name} saknar child med namnet 'Center'");
            Destroy(gameObject);
            return;
        }

        timer = 0f;
        isMoving = true;

        MoveToSplinePosition(0f);
    }

    private void Update()
    {
        if (!isMoving)
            return;
        timer += Time.deltaTime;

        float t = timer / moveDuration;
        t = Mathf.Clamp01(t);

        MoveToSplinePosition(t);
        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Moves the object to a position along the spline at the specified normalized parameter, applying a vertical wave
    /// offset and rotation.
    /// </summary>
    /// <remarks>This method adjusts the object's position and orientation based on the spline's shape and
    /// applies a sinusoidal vertical offset for a wave effect. The object's position is updated relative to a central
    /// reference point, and it is rotated around the Y-axis each time the method is called.</remarks>
    /// <param name="t">The normalized position along the spline, where 0 represents the start and 1 represents the end of the spline.</param>
    private void MoveToSplinePosition(float t)
    {
        float3 localPos = spline.Spline.EvaluatePosition(t);
        Vector3 worldPos = spline.transform.TransformPoint(localPos);

        float sinOffset = Mathf.Sin(t * waveFrequency * Mathf.PI * 2f) * waveAmplitude;

        Vector3 wavePos = worldPos + Vector3.up * sinOffset;

        Vector3 offset = wavePos - center.position;
        transform.position += offset;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform found = FindChildRecursive(child, childName);

            if (found != null)
                return found;
        }

        return null;
    }
}
