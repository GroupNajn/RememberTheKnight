using UnityEngine;

public class EnemyLockOnCanvas : MonoBehaviour
{
    [SerializeField] private Camera worldSpaceCamera;
    [SerializeField] private TargetLockHandler targetLockHandler;

    [SerializeField] private GameObject lockOnIndicatorObject;
    [SerializeField] private Transform indicatorFollowTarget;
    [SerializeField] private Transform lockOnTransform;

    private Canvas canvas;

    private void Start()
    {
        worldSpaceCamera = GameObject.FindWithTag("WorldSpaceCamera").GetComponent<Camera>();
        targetLockHandler = FindFirstObjectByType<TargetLockHandler>();

        lockOnIndicatorObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (indicatorFollowTarget == null)
            return;

        transform.position = indicatorFollowTarget.position;
         
        ShowLockOnIndicator();
    }

    private void ShowLockOnIndicator()
    {
        if (targetLockHandler == null)
            return;

        bool isLockedOn = targetLockHandler.currentTarget == lockOnTransform;

        lockOnIndicatorObject.SetActive(isLockedOn);
    }
}
