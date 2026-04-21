using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractableUIData : MonoBehaviour
{
    public string infoText { get => infoText; private set => infoText = value; }
    public string errorText { get => errorText; private set => errorText = value; } 
    public bool canInteract { get; private set; }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
