using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class TargetLockHandler : MonoBehaviour
{
    public float lockRadius = 15f;
    public LayerMask enemyLayer;

    public Animator cameraAnimator;
    public bool isLockedOn = false;

    public Transform currentTarget;
    public CinemachineTargetGroup targetGroup;
    public Transform playerTransform;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isLockedOn)
            {
                FindTarget();

                if (currentTarget != null)
                    ToggleLock();
            }
            else
            {
                ClearTarget();
                ToggleLock();
            }
        }

        if (isLockedOn && currentTarget == null)
        {
            ClearTarget();
            ToggleLock();
        }

    }

    void ToggleLock()
    {
        isLockedOn = !isLockedOn;
        cameraAnimator.SetBool("IsLockedOn", isLockedOn);
    }

    void FindTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockRadius, enemyLayer);

        Debug.Log("Enemies found: " + enemies.Length);
        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach(Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = enemy.transform;
            }
        }

        if (bestTarget != null)
        {
            currentTarget = bestTarget;
            AddTargets();
        }
    }

    void AddTargets()
    {
        if (currentTarget == null)
            return;

        targetGroup.Targets.Clear();

        targetGroup.AddMember(playerTransform, 1, 1);
        targetGroup.AddMember(currentTarget, 1, 1);
    }

    void ClearTarget()
    {
        currentTarget = null;

        targetGroup.Targets.Clear();
        targetGroup.AddMember(playerTransform, 1, 1);
    }
}
