using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private Camera camera;
    public float InteractDistance = 8f;
    PlayerController playerController;
    [SerializeField] UIManager playerUIManager;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //camera = playerController._playerCamera.GetComponent<Camera>();

    }

    void Update()
    {
        CheckInteractable();
    }

    public void OnInteract()
    {
        camera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * InteractDistance, Color.red, 1f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractDistance, 3))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("TJO KING");
                interactable.Interact();
            }
        }
    }

    public void CheckInteractable()
    {
        camera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractDistance, 3))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (!playerUIManager.UIMenuActive)
                {
                    playerUIManager.OpenInteractiveUI();
                }
                return;
            }
        }

        playerUIManager.CloseInteractiveUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (playerUIManager == null)
        {
            playerUIManager = FindFirstObjectByType<UIManager>();
            Debug.Log("PlayerUIManager not found in the scene after loading. Please ensure there is a PlayerUIManager in the scene.");
        }
    }
}