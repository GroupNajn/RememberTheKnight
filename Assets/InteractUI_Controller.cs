using UnityEngine;
using TMPro;
using System.Collections;

public class InteractUI_Controller : MonoBehaviour
{
    private Canvas canvas;
    private TextMeshProUGUI tmp;
    private GameObject canvasObject;
    private GameObject player;

    private bool canDisplay;
    private bool isShowing;
    private bool coroutineRunning;

    private IInteractable interactable;

    void Start()
    {
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

        if (interactable.IsInteractable && !coroutineRunning)
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

        InitializeTMPText();
        canvasObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        canDisplay = false;
        canvasObject.SetActive(false);
        interactable.IsInteractable = false;

        coroutineRunning = false;
    }

    private void InitializeTMPText()
    {
        var data = interactable.GetUIData();
        tmp.text = data.InfoText;
    }
}