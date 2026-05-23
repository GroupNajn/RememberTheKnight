using Unity.Cinemachine;
using UnityEngine;

public class Sensitivity_Manager : MonoBehaviour
{
    private CinemachineInputAxisController axisController;
    public float mouseSensitivity = 1f;
    public float contollerSensitivity = 3f;

    
 
    void Start()
    {
        axisController = GetComponent<CinemachineInputAxisController>();
        Event_System.instance.OnDeviceChanged += HandleDeviceChanged;
        HandleDeviceChanged();


    }
    private void OnEnable()
    {
        if (Event_System.instance != null)
            Event_System.instance.OnDeviceChanged += HandleDeviceChanged;
    }

    private void OnDisable()
    {
        if (Event_System.instance != null)
            Event_System.instance.OnDeviceChanged -= HandleDeviceChanged;
    }


    public void OnSliderChanged(float value) // called by sensitivity slider in settings menu (1 is default sensitivity)
    {
        if(InputManager.Instance.usingGamepad) // automaticly sets sensitivity based on current input device
        {
            contollerSensitivity = value; // if using gamepad, set controller sensitivity
        }
        else
        {
            mouseSensitivity = value; // if using mouse, set mouse sensitivity
        }

        HandleDeviceChanged(); // check current input and set sensitivity accordingly
    }


    private void HandleDeviceChanged()
    {
        float sensitivity = InputManager.Instance.usingGamepad ? contollerSensitivity : mouseSensitivity;
        SetSensitivity(sensitivity);
    }

    private void SetSensitivity(float sensitivity)
    {
        foreach (var axis in axisController.Controllers)
        {
            if (axis.Name == "Look Orbit X")
            {
                axis.Input.Gain = 1 * sensitivity;
            }

            if (axis.Name == "Look Orbit Y")
            {
                axis.Input.Gain = -1 * sensitivity;
            }
        }
    }
}
