using System.Collections.Generic;
using UnityEngine;

public class ShopSlotCard : MonoBehaviour
{
    [SerializeField] private bool unlocked;

    private List<GameObject> posters = new ();

    void Start()
    {
        posters = GetComponentInParent<List<GameObject>>();
    }
    public void SetCard(CardData card)
    {
        if (!unlocked)
        {

        }
        Debug.Log($"Slot {name} card {card.name}");

        Instantiate(card, this.transform);
    }
}
