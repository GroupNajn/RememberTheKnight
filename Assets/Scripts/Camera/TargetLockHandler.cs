using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Linq;

public class TargetLockHandler : MonoBehaviour
{
    public float lockRadius = 15f;
    public LayerMask enemyLayer;

    public Animator cameraAnimator;
    public bool isLockedOn = false;

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
            if (!isLockedOn)
            {
                FindTarget();

                if (currentTarget != null)
                {
                    isLockedOn = true;
                    SwitchCams();
                }
            }
            else
            {
                Unlock();
            }
        }

        if (isLockedOn && currentTarget == null)
        {
            Unlock();
        }

    }

    void Unlock()
    {
        ClearTarget();
        isLockedOn = false;
        SwitchCams();
    }

    void ToggleLock()
    {
        isLockedOn = !isLockedOn;
        cameraAnimator.SetBool("IsLockedOn", isLockedOn);
    }

    void FindTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(playerTransform.position, lockRadius, enemyLayer);

        Debug.Log("Enemies found: " + enemies.Length);
        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach(Collider enemy in enemies)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = enemy.transform;
            }
        }

        if (bestTarget != null)
        {
            //Transform enemyLockOn = bestTarget.gameObject.GetComponentInChildren<Transform>().Find("EnemyLockOn");

            currentTarget = bestTarget;
            AddTargets();
        }
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
            axisControllerFreeLook.enabled = !isLockedOn;
        }

        if (isLockedOn)
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
