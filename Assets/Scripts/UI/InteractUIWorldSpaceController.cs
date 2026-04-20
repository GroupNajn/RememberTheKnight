using UnityEngine;
using TMPro;
public class InteractableUI_WorldSpace_Controller : MonoBehaviour
{
    private Transform spawnTransform;
    private Canvas canvas;
    private TextMeshProUGUI tmp;
    private IInteractable parent;
    void Start()
    {
        spawnTransform = GameObject.Find("CanvasSpawnPoint").GetComponent<Transform>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        canvas.gameObject.SetActive(false);
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        parent = gameObject.GetComponentInParent<IInteractable>();
        //this.transform.LookAt(Camera.main.transform);
    }



    void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.rotation *= Quaternion.Euler(0f, 180, 0f);
    }

    public void DisplayText(bool isDisplayingInfo)
    {
        if (isDisplayingInfo)
        {
            DisplayInfoText();

        }
        else
        {
            DisplayErrorText();

        }


    }


    private void DisplayInfoText()
    {

    }

    private void DisplayErrorText()
    {

    }
}
