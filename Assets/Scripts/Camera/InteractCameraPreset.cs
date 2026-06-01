using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractCameraPreset", menuName = "Camera/Interact Camera Preset")]
public class InteractCameraPreset : ScriptableObject
{
    /// <summary>
    /// Created by Anton 2026-05-03
    /// Initially created as a ScriptableObject to store camera presets for interactions. 
    /// This allows for easy configuration of camera angles and positions for different interactable objects in the scene.
    /// </summary>

    public Vector3 rotation;
    public Vector2 screenPosition;
    public float distance;
    public float damping = 1f;
}
