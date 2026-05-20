using UnityEngine;

public class FogWallSkull : MonoBehaviour
{
    [SerializeField] float maxXRotation = 50f;
    [SerializeField] float rotationSpeed = 1f;
    GameObject player;
    Quaternion initialRotation;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (player)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            Vector3 euler = transform.rotation.eulerAngles;
            euler.x = Mathf.Clamp(euler.x, -maxXRotation, maxXRotation);
            transform.rotation = Quaternion.Euler(euler);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}