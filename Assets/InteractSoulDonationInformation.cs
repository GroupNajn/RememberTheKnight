using UnityEngine;

public class InteractSoulDonationInformation : MonoBehaviour, IInteractable, IInteractableUIText
{
    private bool isFirstCard = true;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
     public void Interact()
    {
        return;
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
            //isFirstCard = false;
            int amount = GameObject.Find("GlobalData").GetComponent<GameData>().SoulsRemainingToNextUnlock;

        UIData.InfoText = $"Souls until next unlock {amount}";
        return UIData;
    }
}
