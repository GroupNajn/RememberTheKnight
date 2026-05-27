using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateCharacter : MonoBehaviour
{
    float rotationSpeed = -300f;
    float horizontalInput;

    [SerializeField] GameObject playerObject;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != SceneData.Instance[1])
        {
            // Only allow rotation when in character select screen
            return;
        }

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            horizontalInput = Input.GetAxis("Mouse X");
            playerObject.transform.Rotate(0f, horizontalInput * rotationSpeed * Time.unscaledDeltaTime, 0f, Space.Self);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}