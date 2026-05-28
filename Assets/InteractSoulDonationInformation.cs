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
        GameData data = GameObject.Find("GlobalData").GetComponent<GameData>();
        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        if(collection.playerContract == null || collection.playerContract.CardFamily == CardFamily.None)
        {
            UIData.InfoText = $"No Contract Signed";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Wands)
        {
            UIData.InfoText = $"Souls until next Wands Unlock:{data.SoulsRemainingToNextUnlockWands}";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Cups)
        {
            UIData.InfoText = $"Souls until next Cups Unlock:{data.SoulsRemainingsoulToNextUnlockCups}";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Pentacles)
        {
            UIData.InfoText = $"Souls until next Pentacles Unlock:{data.SoulsRemainingToNextUnlockPentacles}";
        }
        else if(collection.playerContract.CardFamily == CardFamily.Swords)
        {
            UIData.InfoText = $"Souls until next Swords Unlock:{data.SoulsRemainingToNextUnlockSwords}";
        }
            //isFirstCard = false;
            

        return UIData;
    }
}
