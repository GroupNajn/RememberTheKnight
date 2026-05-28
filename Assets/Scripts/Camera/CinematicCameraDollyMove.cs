using Unity.Cinemachine;
using UnityEngine;
public class CinematicCameraDollyMove : MonoBehaviour
{
    public CinemachineSplineDolly dolly;
    public float speed = 1f;

    private void Update()
    {
        dolly.CameraPosition += speed * Time.deltaTime;
    }
}
