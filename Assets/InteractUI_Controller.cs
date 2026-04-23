using UnityEngine;
using TMPro;
using System.Collections;

public class InteractUI_Controller : MonoBehaviour
{
    private Canvas canvas;
    private TextMeshProUGUI tmp;
    private GameObject canvasObject;
    private GameObject player;
    private float displayDistance;

    private bool forceDisabled;
    private bool isShowing;

    private IInteractableUI interactableUI;

    void Start()
    {

        displayDistance = 4;
        canvas = GetComponentInChildren<Canvas>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();

        canvasObject = canvas.gameObject;
        canvasObject.SetActive(false);

        canvas.worldCamera = Camera.main;

        player = GameObject.FindWithTag("Player");
        interactableUI = GetComponent<IInteractableUI>();
        InitializeTMPText(interactableUI.GetUIData());


    }

    void Update()
    {

        Vector3 direction = transform.position - player.transform.position;

        ForceDisable();
        if (direction.magnitude > displayDistance && isShowing)
        {
            StartCoroutine(StopShowRoutine());
        }



        if (canvasObject.activeInHierarchy)
        {
            Vector3 cameraDirection = Camera.main.transform.position - canvas.transform.position;
            cameraDirection.y = 0;

            canvas.transform.rotation = Quaternion.LookRotation(cameraDirection) * Quaternion.Euler(0, 180, 0);
        }
    }

    // A coroutine to start the display duration, which currently is 5 seconds. 
    // After 5 seconds the canvasObject should exit gracefully. Props to Farid. 
    public void EnableCanvasObject()
    {
        isShowing = true;
        if (canvasObject.activeInHierarchy || forceDisabled) return;
        canvasObject.SetActive(true);

    }

    public IEnumerator StopShowRoutine()
    {
        yield return new WaitForSeconds(5f);

        canvasObject.SetActive(false);
    }

    public void DisableCanvas()
    {
        canvasObject.SetActive(false);
        isShowing = false;
        return;
    }


    private void ForceDisable()
    {
        if (forceDisabled)
        {
            isShowing = false;
            canvasObject.SetActive(false);
        }
    }


    public void InitializeTMPText(InteractableUIData UIData)
    {
        tmp.text = UIData.InfoText;
    }
}