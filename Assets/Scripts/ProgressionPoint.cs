using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ProgressionPoint : MonoBehaviour
{
    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        boxCollider.enabled = true;
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Event_System.instance.OnLoadScenes.Invoke();
    }
}