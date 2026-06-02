using System.Security;
using UnityEngine;

public class BossCardInstantiater : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] CardData[] cards;
    private CardBuilder cardBuilder;
    private PlayerCollection playerCollection;

    void Start()
    {
  
        SetCardData();
    }
    private void Awake()
    {
        //Event_System.instance.OnBossDeath += EnableObject;
    }

    void Update()
    {

    }
    private void OnEnable()
    {
        SetCardData();
    }
    private void OnDestroy()
    {
        //Event_System.instance.OnBossDeath -= EnableObject;
    }


    private void EnableObject()
    {
        gameObject.SetActive(true);
    }

    private void SetCardData()
    {
        Card cardScript = cardPrefab.GetComponent<Card>();
        cardBuilder = GameObject.Find("CardSystem").GetComponent<CardBuilder>();
        playerCollection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        foreach (CardData cardData in cards)
        {
            if(playerCollection.playerContract == null)
            {
                int rndIndex = Random.Range(0, cards.Length);
                cardBuilder.SetCardMaterial(cards[rndIndex], cardScript);
                cardScript.SetCardData(cards[rndIndex]);
                return;

            }
            if (cardData.cardFamily == playerCollection.playerContract.CardFamily)
            {
                cardBuilder.SetCardMaterial(cardData, cardScript);
                cardScript.SetCardData(cardData);
            }
        }

    }
}
