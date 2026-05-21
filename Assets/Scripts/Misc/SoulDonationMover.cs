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

    private void MoveToSplinePosition(float t)
    {
        float3 localPos = spline.Spline.EvaluatePosition(t);
        Vector3 worldPos = spline.transform.TransformPoint(localPos);

        Vector3 offset = worldPos - center.position;
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