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
        int amount = 0;
        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        if(collection.playerContract == null || collection.playerContract.CardFamily == CardFamily.None)
        {
            UIData.InfoText = $"No Contract Signed";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Wands)
        {
            UIData.InfoText = $"Souls until next Wand Unlock:{amount}";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Cups)
        {
            UIData.InfoText = $"Souls until next Cups Unlock:{amount}";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Pentacles)
        {
            UIData.InfoText = $"Souls until next Pentacles Unlock:{amount}";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Swords)
        {
            UIData.InfoText = $"Souls until next Pentacles Unlock:{amount}";
        }
            //isFirstCard = false;
            

        return UIData;
    }
}
