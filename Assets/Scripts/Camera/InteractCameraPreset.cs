using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractCameraPreset", menuName = "Camera/Interact Camera Preset")]
public class InteractCameraPreset : ScriptableObject
{
    public Vector3 rotation;
    public Vector2 screenPosition;
    public float distance;
    public float damping = 1f;
}
