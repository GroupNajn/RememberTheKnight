using Unity.AppUI.UI;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera camera;
    public float InteractDistance = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
    }

    public void OnInteract()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * InteractDistance, Color.red, 1f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractDistance,3))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}