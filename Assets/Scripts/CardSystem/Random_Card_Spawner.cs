using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine.SceneManagement;

public class Random_Card_Spawner : MonoBehaviour
{
    [SerializeField] List<CardData> secretCards;
    [SerializeField] List<CardData> devilCards;   

    [SerializeField] private CardBuilder builder;
    [SerializeField] private RunGameData gameData;
    [SerializeField] private Transform spawnTransform;

    [SerializeField] private CardSpawnTypeSecret cardSpawnType;

    [SerializeField] private float chanceToSpawnCard = 0.5f;
    void Start()
    {
        builder = GameObject.Find("CardSystem").GetComponent<CardBuilder>();
        gameData = GameObject.Find("GlobalData").GetComponent<RunGameData>();
        SpawnCard();
    }

    void Update()
    {

    }

    public void SpawnCard()
    {
        if (Random.value > chanceToSpawnCard)
        {
            return;
        }

        if (!gameData.HasSecretOneBeenPickedUp && cardSpawnType == CardSpawnTypeSecret.Secret)
        {
            int index = Random.Range(0, secretCards.Count);

            CardData card = secretCards[index];
            builder.InstantiateSecretCard(card, spawnTransform);
            return;
        }

        if (!gameData.HasSecretTwoBeenPickedUp && cardSpawnType == CardSpawnTypeSecret.Devil)
        {
            int index = Random.Range(0, devilCards.Count);

            CardData card = devilCards[index];
            builder.InstantiateSecretCard(card, spawnTransform);
            return;
        }
    }
}
