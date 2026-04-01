using UnityEngine;

public class PlayerLockRotation : MonoBehaviour
{

    public TargetLockHandler lockHandler;
    public float rotationSpeed = 10f;
    public bool RotationEnabled = false;

    void LateUpdate()
    {
        if (RotationEnabled)
        {
            if (lockHandler.currentTarget == null)
                return;

            Vector3 direction = lockHandler.currentTarget.position - transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
