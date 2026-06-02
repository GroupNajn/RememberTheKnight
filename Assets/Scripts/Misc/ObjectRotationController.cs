using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private float rotatioSpeed = 30f;

    // Update is called once per frame
    void Update()
    {
        objectToRotate.transform.Rotate(Vector3.up * Time.deltaTime * rotatioSpeed);
    }
}
