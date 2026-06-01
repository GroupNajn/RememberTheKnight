using System.Collections.Generic;
using UnityEngine;

public class HighlightTarget : MonoBehaviour
{
    /// <summary>
    /// Created by Anton 2026-05-12
    /// Initially created as a component to handle the highlighting of objects when the player interacts with them, or hovers over them.
    /// </summary>

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
