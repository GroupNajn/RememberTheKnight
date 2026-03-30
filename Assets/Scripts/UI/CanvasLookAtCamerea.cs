using UnityEngine;

public class CanvasLookAtCamerea : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}