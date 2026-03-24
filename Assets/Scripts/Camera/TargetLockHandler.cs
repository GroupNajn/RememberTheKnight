using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Linq;
using Unity.Behavior;

public class TargetLockHandler : MonoBehaviour
{
    public float lockRadius = 15f;
    public LayerMask enemyLayer;

    public Animator cameraAnimator;
    public bool IsLockedOn = false;

    public Transform currentTarget;
    public CinemachineTargetGroup targetGroup;
    public Transform playerTransform;
    public Transform testCubeTransform;

    [Header(header: "Cameras")]
    [SerializeField] private GameObject freeLookCam;
    [SerializeField] private GameObject hardlockCam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!IsLockedOn)
            {
                FindTarget();

                if (currentTarget != null)
                {
                    IsLockedOn = true;
                    SwitchCams();
                }
            }
            else
            {
                Unlock();
            }
        }

        if (IsLockedOn)
        {
            if (currentTarget != null && !currentTarget.gameObject.GetComponent<BehaviorGraphAgent>().enabled)
            {
                FindTarget();
            }
            if (currentTarget == null)
            {
                FindTarget();
            }
            if (currentTarget == null) { Unlock(); }
        }

    }

    void Unlock()
    {
        ClearTarget();
        IsLockedOn = false;
        SwitchCams();
    }

    void ToggleLock()
    {
        IsLockedOn = !IsLockedOn;
        cameraAnimator.SetBool("IsLockedOn", IsLockedOn);
    }

    void FindTarget()
    {
        List<Collider> enemiesUnfiltered = new(Physics.OverlapSphere(playerTransform.position, lockRadius, enemyLayer));
        List<Collider> enemies = new();

        enemiesUnfiltered.ForEach(enemy =>
        {
            var agent = enemy.gameObject.GetComponent<BehaviorGraphAgent>();
            if (agent != null && agent.enabled)
                enemies.Add(enemy);
        });

        Debug.Log("Enemies found: " + enemies.Count);
        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = enemy.transform;
            }
        }


        currentTarget = bestTarget;
        AddTargets();

    }

    void AddTargets()
    {
        if (currentTarget == null)
            return;

        targetGroup.Targets.Clear();

        targetGroup.AddMember(playerTransform, 0.75f, 1f);
        //targetGroup.AddMember(testCubeTransform, 0.75f, 1f);
        targetGroup.AddMember(currentTarget, 1f, 1);
    }

    void ClearTarget()
    {
        currentTarget = null;

        targetGroup.Targets.Clear();
        //targetGroup.AddMember(testCubeTransform, 0.75f, 1f);
        //targetGroup.AddMember(playerTransform, 0.75f, 1f);
    }

    private void SwitchCams()
    {
        CinemachineInputAxisController axisControllerFreeLook = freeLookCam.GetComponent<CinemachineInputAxisController>();
        CinemachineCamera cinemachineFreeLookCam = freeLookCam.GetComponent<CinemachineCamera>();
        CinemachineCamera cinemachineHardLockCam = hardlockCam.GetComponent<CinemachineCamera>();
        CinemachineGroupFraming cinemachineHardLockCamGroupFraming = hardlockCam.GetComponent<CinemachineGroupFraming>();

        if (axisControllerFreeLook != null)
        {
            axisControllerFreeLook.enabled = !IsLockedOn;
        }

        if (IsLockedOn)
        {
            cinemachineHardLockCam.ForceCameraPosition(pos: cinemachineFreeLookCam.State.GetFinalPosition(), rot: cinemachineFreeLookCam.State.GetFinalOrientation());
            cameraAnimator.Play(stateName: "HardLockCamera");
        }
        else
        {
            cinemachineHardLockCamGroupFraming.Damping = 0;
            cinemachineFreeLookCam.ForceCameraPosition(pos: cinemachineHardLockCam.State.GetFinalPosition(), rot: cinemachineHardLockCam.State.GetFinalOrientation());
            cameraAnimator.Play(stateName: "FreeLookCamera");
        }
    }
}
