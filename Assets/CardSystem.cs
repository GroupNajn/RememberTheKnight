using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class CardSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> allCards = new();
    [SerializeField] private List<CardData> unlockedCards = new();

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public IReadOnlyList<CardData> GetUnlockedCards()
    {
        return unlockedCards;
    }

    //public List <CardData> GetDroppableCards()
    //{
    //    List<CardData> droppableCards = new List<CardData>();

    //    foreach (CardData card in unlockedCards)
    //    {
            
    //    }

    //}

    


}
