using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //[Header("References")]
    //public Transform orientation;
    //public Transform player;
    //public Transform playerObj;

    public float rotationSpeed;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        //Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //orientation.forward = viewDir.normalized;

        //Vector3 targetPos = player.position;
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);

        //Vector3 viewDir = player.position - transform.position;
        //viewDir.y = 0f;
        //orientation.forward = viewDir.normalized;

        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //if (inputDir != Vector3.zero)
        //    playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

    }
}
