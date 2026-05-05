using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetLockHandler : MonoBehaviour
{
    private LayerMask enemyLayer;
    private LayerMask lineOfSightLayer;
    private Animator cameraAnimator;

    private float mouseX;
    private bool isCentringCamera = false;

    [Header(header: "TargetLock Settings")]
    public float lockRadius = 15f;
    public float breakLockDistance = 17.5f;
    public bool IsLockedOn = false;
    public float lockBreakMouseXThreshold = 300f;

    private float lostSightTimer = 0f;
    public float loseSightDelay = 0.1f;

    public bool AutomaticlyFindNewTarget = true;

    [Range(0f, 1f)]
    public float minDotProduct = 0.5f;

    [Header(header: "Targets")]
    public Transform currentTarget;
    private CinemachineTargetGroup targetGroup;
    private Transform playerTransform;

    [Header(header: "Cameras")]
    private GameObject freeLookCam;
    private GameObject hardlockCam;
    private CinemachineCamera cinemachineFreeLookCam;
    private CinemachineCamera cinemachineHardLockCam;

    void Start()
    {
        targetGroup = FindFirstObjectByType<CinemachineTargetGroup>();
        enemyLayer = LayerMask.GetMask("Enemy");
        lineOfSightLayer = LayerMask.GetMask("Environment", "Obstacle");
        cameraAnimator = GetComponentInChildren<Animator>();
        freeLookCam = GameObject.FindGameObjectWithTag("FreeLookCamera");
        hardlockCam = GameObject.FindGameObjectWithTag("HardLockCamera");
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
        Mathf.MoveTowards(mouseX, 0, Time.deltaTime * 300f);

        

        if (IsLockedOn)
        {
            if (mouseX > lockBreakMouseXThreshold || mouseX < -lockBreakMouseXThreshold)
            {
                FindNewTarget();
            }

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
                if (AutomaticlyFindNewTarget)
                    FindTarget();
                else
                    Unlock();
            }
            if (currentTarget == null) { Unlock(); }
        }

    }
    void ForceCenterCamera()
    {
        cinemachineFreeLookCam.ForceCameraPosition(pos: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.position, rot: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.rotation);
        cinemachineHardLockCam.ForceCameraPosition(pos: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.position, rot: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.rotation);
    }
    IEnumerator SmoothCenterCamera()
    {
        float time = 0f;
        float lerpTime = 0.2f;
        cinemachineFreeLookCam.GetComponent<CinemachineDeoccluder>().enabled = false;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            float t = time/ lerpTime;
            Debug.Log("Centring camera...");
            Transform centerdTransform = GameObject.FindGameObjectWithTag("CameraCenteredPos").transform;

            cinemachineFreeLookCam.ForceCameraPosition(Vector3.Lerp(cinemachineFreeLookCam.transform.position, centerdTransform.position, t), Quaternion.Slerp(cinemachineFreeLookCam.transform.rotation, centerdTransform.rotation, t));

            //if (Quaternion.Angle(cinemachineFreeLookCam.transform.rotation.normalized, centerdTransform.rotation.normalized) < 2f && Vector3.Distance(cinemachineFreeLookCam.transform.position, centerdTransform.position) < 0.9)
            //{
            //    cinemachineFreeLookCam.ForceCameraPosition(pos: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.position, rot: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.rotation);
            //    isCentringCamera = false;
            //}
            yield return null;

        }
        //cinemachineFreeLookCam.ForceCameraPosition(pos: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.position, rot: GameObject.FindGameObjectWithTag("CameraCenteredPos").transform.rotation);
        cinemachineFreeLookCam.GetComponent<CinemachineDeoccluder>().enabled = true;

        // yield return null;
    }
    //void SmoothCenterCamera()
    //{
    //    isCentringCamera = true;
    //}
    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, lockRadius, enemyLayer);
        List<Collider> enemiesInRange = new();
        HashSet<Transform> seenTargets = new();

        foreach (Collider hit in hits)
        {
            BehaviorGraphAgent agent = hit.GetComponentInParent<BehaviorGraphAgent>();

            if (agent == null || !agent.enabled)
                continue;

            Transform enemyTransform = agent.transform;


            if (Vector3.Distance(enemyTransform.position, playerTransform.position) > lockRadius)
                continue;

            if (seenTargets.Add(enemyTransform))
            {
                enemiesInRange.Add(hit);
            }
        }

        if (enemiesInRange.Count == 0) // No valid targets found, just center the camera and return
        {
            //SmoothCenterCamera();
            StartCoroutine(SmoothCenterCamera());
            return;
        }

        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider enemy in enemiesInRange)
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
    private void FindNewTarget()
    {
        // Debug.Log("FIND NEW TARGETS CALLED");

        Collider[] hits = Physics.OverlapSphere(playerTransform.position, lockRadius, enemyLayer);
        List<Collider> enemiesInRange = new();

        HashSet<Transform> seenTargets = new();

        // Debug.Log("Enemy Unfiltered found " + hits.Length);

        // Collect unique enemies
        foreach (Collider hit in hits)
        {
            BehaviorGraphAgent agent = hit.GetComponentInParent<BehaviorGraphAgent>();

            if (agent == null || !agent.enabled)
                continue;

            Transform enemyTransform = agent.transform;

            if (Vector3.Distance(enemyTransform.position, playerTransform.position) > lockRadius)
                continue;

            if (seenTargets.Add(enemyTransform))
            {
                enemiesInRange.Add(hit);
                //   Debug.Log("Enemy Added to potential targets: " + enemyTransform.name);
            }
        }

        // Debug.Log("Enemies in range after first filter: " + enemiesInRange.Count);

        // Filter by direction / line of sight / current target
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            Transform enemy = enemiesInRange[i].transform;
            Vector3 directionToEnemy = (enemy.position - playerTransform.position).normalized;

            float dotfwd = Vector3.Dot(Camera.main.transform.forward, directionToEnemy);

            if (dotfwd < minDotProduct || currentTarget == enemy)
            {
                //      Debug.Log("Enemy Removed due outside minDot or already being target: " + enemy.name);
                enemiesInRange.RemoveAt(i);
                continue;
            }

            float dotright = Vector3.Dot(Camera.main.transform.right, directionToEnemy);

            if (mouseX < 0 && dotright >= 0)
            {
                //     Debug.Log("Enemy Removed due to being right of you: " + enemy.name);
                enemiesInRange.RemoveAt(i);
                continue;
            }

            if (mouseX > 0 && dotright < 0)
            {
                //     Debug.Log("Enemy Removed due to being left of you: " + enemy.name);
                enemiesInRange.RemoveAt(i);
                continue;
            }
        }

        //  Debug.Log("Enemies in range filtered by direction and line of sight: " + enemiesInRange.Count);

        if (enemiesInRange.Count == 0)
        {
            //        Debug.Log("No valid targets found.");

            return;
        }

        // Pick the best target
        Transform bestTarget = null;
        float lowestDot = Mathf.Infinity;

        foreach (Collider c in enemiesInRange)
        {
            Transform enemy = c.transform;
            Vector3 directionToEnemy = (enemy.position - playerTransform.position).normalized;

            float dotRight = Mathf.Abs(Vector3.Dot(Camera.main.transform.right, directionToEnemy));

            if (dotRight < lowestDot)
            {
                lowestDot = dotRight;
                bestTarget = enemy;
                //        Debug.Log("New best target: " + enemy.name);
            }
        }

        //  Debug.Log("Best target: " + bestTarget.name);

        currentTarget = bestTarget;
        AddTargets();
        mouseX = 0f;
    }
    void ToggleLock()
    {
        IsLockedOn = !IsLockedOn;
        cameraAnimator.SetBool("IsLockedOn", IsLockedOn);
    }
    void Unlock()
    {
        ClearTarget();
        IsLockedOn = false;
        SwitchCams();
        mouseX = 0f;
    }
    bool HasLineOfSight(Transform target)
    {
        Vector3 origin = Camera.main.transform.position;

        Collider col = target.GetComponent<Collider>();
        Vector3 targetPoint = col != null ? col.bounds.center : target.position;

        Vector3 direction = targetPoint - origin;
        float distance = direction.magnitude;
        if (distance > lockRadius)
        {
            return false; // If the target is beyond lock radius, we can immediately return false without doing a raycast
        }

        bool hitSomething = Physics.Raycast(origin, direction.normalized, distance, lineOfSightLayer);

        return !hitSomething;

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
    private void OnTarget()
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
    private void OnLook(InputValue context)
    {
        mouseX = context.Get<float>();
    }
    void OnSwitchTargetRight()
    {
        if (IsLockedOn)
        {
            mouseX = lockBreakMouseXThreshold + 1f;
        }
    }
    void OnSwitchTargetLeft()
    {
        if (IsLockedOn)
        {
            mouseX = -lockBreakMouseXThreshold - 1f;
        }
    }
}