using UnityEngine;

public interface IInteractable
{
    void Interact();

    // Info string is used in the logic of displaying an information string above the object
    // if it is set to null = in the script that implements IInteractable it will not activate the UI element. 
    // SET "InfoString" = null if you dont want an object to have an information display. 
    public string InfoString { get;}

    

}
