using System.Runtime.CompilerServices;
using UnityEngine;

public class LockOnUi : MonoBehaviour
{
    private TargetLockHandler lockHandler;
    private Camera mainCamera;
    private RectTransform indicatorRect;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2f, 0);

    private void Start()
    {
        GameObject rect = GameObject.FindGameObjectWithTag("LockOnIndicator");
        mainCamera = Camera.main;
        lockHandler = GetComponent<TargetLockHandler>();
        indicatorRect = rect.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (lockHandler == null || mainCamera == null || indicatorRect == null)
            return;

        if (!lockHandler.IsLockedOn || lockHandler.currentTarget == null)
        {
            indicatorRect.gameObject.SetActive(false);
            return;
        }

        Vector3 targetWorldPos = lockHandler.currentTarget.position + worldOffset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetWorldPos);

        if (screenPos.z <= 0f)
        {
            indicatorRect.gameObject.SetActive(false);
            return;
        }

        indicatorRect.gameObject.SetActive(true);
        indicatorRect.position = screenPos;
    }
}
