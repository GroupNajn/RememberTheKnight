using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;
using UnityEngine.UI;

public class CardSlotShopUI : MonoBehaviour
{
    [field: SerializeField] public bool IsSelected { get; private set; }
    [field: SerializeField] public bool IsUnlockable { get; private set; } = false;
    [SerializeField] public bool isLocked;

    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.white;

    [SerializeField] private CardData cardData;
    public CardData CardData => cardData;

    void Awake()
    {
        IsSelected = false;

    }
    void Start()
    {
        IsSelected = false;
    }
    // Added IsSelected = false becuase the first time the card is started via 
    // UIManager it is set to false to default to that, once it's state has been updated once during the game.
    // It will no longe be reset to false. 

    public void SetCard(CardData card)
    {
        if (isLocked)
        {
            cardData = null;
        }

        if (card == null)
            return;

        cardData = card;
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }

    public void SetUnlockable(bool unlockable)
    {
        this.IsUnlockable = unlockable;
    }
}

