using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Linq;
using Unity.Behavior;

public class TargetLockHandler : MonoBehaviour
{ 
    public LayerMask enemyLayer;
    public LayerMask lineOfSightLayer;
    public Animator cameraAnimator;

    [Header(header: "TargetLock Settings")]
    public float lockRadius = 15f;
    public float breakLockDistance = 17.5f;
    public bool IsLockedOn = false;

    private float lostSightTimer = 0f;
    public float loseSightDelay = 0.1f;

    [Range(0f, 1f)]
    public float minDotProduct = 0.5f;

    [Header(header: "Targets")]
    public Transform currentTarget;
    public CinemachineTargetGroup targetGroup;
    public Transform playerTransform;
    public Transform testCubeTransform;

    [Header(header: "Cameras")]
    [SerializeField] private GameObject freeLookCam;
    [SerializeField] private GameObject hardlockCam;
    private CinemachineCamera cinemachineFreeLookCam;
    private CinemachineCamera cinemachineHardLockCam;


    void Start()
    {
        cinemachineFreeLookCam = freeLookCam.GetComponent<CinemachineCamera>();
        cinemachineHardLockCam = hardlockCam.GetComponent<CinemachineCamera>();

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerLookAt");

            cinemachineFreeLookCam.Follow = playerTransform;
            cinemachineHardLockCam.Follow = playerTransform;
        }

    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5f, Color.red);
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
            if (currentTarget != null)
            {
                float distance = Vector3.Distance(playerTransform.position, currentTarget.position);

                if (distance > breakLockDistance)
                {
                    Unlock();
                    return;
                }

                //Testing with lineofsight breaking lock.

                if (!HasLineOfSight(currentTarget))
                {
                    lostSightTimer += Time.deltaTime;

                    if (lostSightTimer >= loseSightDelay)
                    {
                        Unlock();
                        return;
                    }
                }
                else
                {
                    lostSightTimer = 0f;
                }
            }
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

    bool HasLineOfSight(Transform target)
    {
        Vector3 origin = Camera.main.transform.position;

        Collider col = target.GetComponent<Collider>();
        Vector3 targetPoint = col != null ? col.bounds.center : target.position;

        Vector3 direction = targetPoint - origin;
        float distance = direction.magnitude;

        bool hitSomething = Physics.Raycast(origin, direction.normalized, distance, lineOfSightLayer);

        return !hitSomething;

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
            Vector3 directionToEnemy = (enemy.transform.position - playerTransform.position).normalized;

            Vector3 cameraForward = Camera.main.transform.forward;

            float dot = Vector3.Dot(cameraForward, directionToEnemy);

            if (dot < minDotProduct)
                continue;

            if (!HasLineOfSight(enemy.transform))
                continue;

            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = enemy.transform;
                //Debug.Log("Enemies in range: " + enemiesUnfiltered.Count);
                //Debug.Log("Filtered enemies: " + enemies.Count);
                //Debug.Log("Best target: " + bestTarget);
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
        targetGroup.AddMember(currentTarget, 1f, 1);

        currentTarget.gameObject.GetComponentInChildren<EnemyHealthBarCanvas>().ShowHealthBar();
    }

    void ClearTarget()
    {
        if (currentTarget != null)
            currentTarget.gameObject.GetComponentInChildren<EnemyHealthBarCanvas>().HideHealthBar();

        currentTarget = null;

        targetGroup.Targets.Clear();
    }

    private void SwitchCams()
    {
        CinemachineInputAxisController axisControllerFreeLook = freeLookCam.GetComponent<CinemachineInputAxisController>();
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
