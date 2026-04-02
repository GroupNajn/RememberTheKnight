using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    float rotationSpeed = -300f;
    float horizontalInput;

    [SerializeField] GameObject playerObject;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            horizontalInput = Input.GetAxis("Mouse X");
            playerObject.transform.Rotate(0f, horizontalInput * rotationSpeed * Time.deltaTime, 0f, Space.Self);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }



}
