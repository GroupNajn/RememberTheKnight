using UnityEngine;

public class EnemyLockOnCanvas : MonoBehaviour
{
    /// <summary>
    /// Created by Anton 2026-05-14
    /// Initially created as a component to handle the lock on indicator for enemies, 
    /// which is a canvas that is attached to the enemy and shows an indicator when the player locks on to the enemy.
    /// 
    /// More robust system compared to other lock on indicators, 
    /// as it uses a world space canvas that is attached to the enemy, 
    /// and follows the enemy around.
    /// </summary>

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
