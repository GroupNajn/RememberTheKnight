using UnityEngine;

public class InteractableUI_WorldSpace_Controller : MonoBehaviour
{
    private Transform spawnTransform;
    private Canvas canvas;
    void Start()
    {
        spawnTransform = GameObject.Find("CanvasSpawnPoint").GetComponent<Transform>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        canvas.gameObject.SetActive(false);
        //this.transform.LookAt(Camera.main.transform);
    }

    void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.rotation *= Quaternion.Euler(0f, 180, 0f);
    }


    private void DisplayInfoText()
    {

    }

    private void DisplayErrorText()
    {

    }
}
