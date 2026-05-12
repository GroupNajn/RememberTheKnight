using System.Collections.Generic;
using UnityEngine;

public class HighlightTarget : MonoBehaviour
{
    [SerializeField] private List<GameObject> highlightedObjects = new();

    private int originalLayer;
    private int outlineLayer;

    private void Awake()
    {
        originalLayer = gameObject.layer;
        outlineLayer = LayerMask.NameToLayer("Outline");
    }

    public void SetHighlight(bool active)
    {
        int targetLayer = active ? outlineLayer : originalLayer;

        foreach (GameObject obj in highlightedObjects)
        {
            if (obj != null)
            {
                obj.layer = targetLayer;
            }
        }
    }
}
