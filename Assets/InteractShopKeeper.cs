using UnityEngine;
using UnityEngine.AI;

public class InteractShopKeeper : MonoBehaviour, IInteractable, IInteractableUIText
{
    private InteractCardShop cardShop;

    void Start()
    {
        cardShop = GameObject.Find("ShopBoard").GetComponent<InteractCardShop>();

    }

    void Update()
    {

    }

    public void Interact()
    {

    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        if (!cardShop.HasBoughtCard)
        {
            UIData.InfoText = $"Talk with the shop keeper.";
        }
        //else if (cardShop.HasBoughtCard)
        //{
        //    UIData.InfoText = $"";
        //}


        return UIData;


    }

}

