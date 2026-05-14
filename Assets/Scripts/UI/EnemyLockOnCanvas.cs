using UnityEngine;

public class EnemyLockOnCanvas : MonoBehaviour
{
    [SerializeField] private Camera worldSpaceCamera;
    [SerializeField] private TargetLockHandler targetLockHandler;

    [SerializeField] private GameObject lockOnIndicatorObject;
    [SerializeField] private Transform followTarget;

    private Canvas canvas;

    private void Start()
    {
        worldSpaceCamera = GameObject.FindWithTag("WorldSpaceCamera").GetComponent<Camera>();
        targetLockHandler = FindFirstObjectByType<TargetLockHandler>();

        //canvas = GetComponent<Canvas>();
        //canvas.worldCamera = canvasCamera;

        lockOnIndicatorObject.SetActive(false);
    }

    private void Update()
    {
        if (followTarget == null)
            return;

        transform.position = followTarget.position;

        transform.LookAt(worldSpaceCamera.transform.position);

        ShowLockOnIndicator();
    }

    private void ShowLockOnIndicator()
    {
        if (targetLockHandler == null)
            return;

        bool isLockedOn = targetLockHandler.currentTarget == followTarget;

        lockOnIndicatorObject.SetActive(isLockedOn);
    }
}
