using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class Random_Card_Spawner : MonoBehaviour
{
    [SerializeField] List<CardData> secretCards;

    [SerializeField] private CardBuilder builder;
    [SerializeField] private RunGameData gameData;
    [SerializeField] private Transform spawnTransform;
    void Start()
    {
        builder = GameObject.Find("CardSystem").GetComponent<CardBuilder>();
        gameData = GameObject.Find("GlobalData").GetComponent<RunGameData>();
        SpawnCard(gameData.hasSpawnedSecretCard);
    }

    void Update()
    {

    }

    public void SpawnCard(bool hasSpawnedSecret)
    {
        if (!hasSpawnedSecret)
        {
            int index = Random.Range(0, secretCards.Count);
            CardData card = secretCards[index];
            builder.InstantiateSecretCard(card, spawnTransform);
            gameData.hasSpawnedSecretCard = true;
        }
    }



}
