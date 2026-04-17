using UnityEngine;

public class InteractableUI_WorldSpace_Controller : MonoBehaviour
{
    private Transform spawnTransform;
    private Canvas canvas;
    void Start()
    {
        spawnTransform = GameObject.Find("CanvasSpawnPoint").GetComponent<Transform>();
        canvas = GetComponent<Canvas>();
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.LookAt(Camera.main.transform);
    }

    void Update()
    {


    }


    private void DisplayInfoText()
    {

    }

    private void DisplayErrorText()
    {

    }
}
