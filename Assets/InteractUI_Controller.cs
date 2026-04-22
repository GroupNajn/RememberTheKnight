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

    private bool canDisplay;
    private bool isShowing;
    private bool coroutineRunning;

    private IInteractable interactable;

    void Start()
    {

        displayDistance = 4;
        canvas = GetComponentInChildren<Canvas>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();

        canvasObject = canvas.gameObject;
        canvasObject.SetActive(false);

        canvas.worldCamera = Camera.main;

        player = GameObject.FindWithTag("Player");
        interactable = GetComponent<IInteractable>();

        InitializeTMPText();
    }

    void Update()
    {
        if (interactable == null || player == null)
            return;

        if (!interactable.IsLookedAt)
        {
            OverrideDisplayDuration();
        }

        if (interactable.IsLookedAt && !coroutineRunning)
        {
            StartCoroutine(ShowUIRoutine());
        }

        if (canvasObject.activeInHierarchy)
        {
            Vector3 direction = transform.position - player.transform.position;
            canvas.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private IEnumerator ShowUIRoutine()
    {
        coroutineRunning = true;
        canDisplay = true;
        isShowing = true;
        

        InitializeTMPText();
        canvasObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        Vector3 direction = transform.position - player.transform.position;

        if (direction.magnitude > displayDistance || !canDisplay)
        {
            canDisplay = false;
            isShowing = false;
            canvasObject.SetActive(false);
            interactable.IsLookedAt = false;
            coroutineRunning = false;

        }
        coroutineRunning = false;

    }

    private void OverrideDisplayDuration()
    {
        coroutineRunning = false;
        canDisplay = false;
        isShowing = false;
        canvasObject.SetActive(false);


    }

    private void InitializeTMPText()
    {
        var data = interactable.GetUIData();
        tmp.text = data.InfoText;
    }
}