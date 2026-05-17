using FMODUnity;
using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static IPickupable;

// Script made by Henric in the end of April 2026.

public class CardBuilder : MonoBehaviour
{
    private Transform parentSpawnPosTrans;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Card cardScript;
    [SerializeField] private CardSystem cardSystem;
    [SerializeField] Transform playerTrans;
    [SerializeField] ParticleSystem[] normalDropParticles = new ParticleSystem[5];
    [SerializeField] ParticleSystem[] shopBoardParticles = new ParticleSystem[5];
    bool keyPressed = false;

    void Start()
    {

    }

    void Update()
    {
    }
    public GameObject InstantiateCardWithoutScripts(CardData cardData, Transform parentTransform)
    {
        Quaternion rotation = Quaternion.Euler(parentTransform.rotation.eulerAngles.x, parentTransform.rotation.eulerAngles.y + 180, parentTransform.rotation.eulerAngles.z);

        GameObject cardInstance = Instantiate(cardPrefab, parentTransform.position, rotation, parentTransform);
        Instantiate(ReturnParticleShopboard(LootManager.instance.GetRarityFromTier(cardData.cardTier)), cardInstance.transform.position, Quaternion.identity, cardInstance.transform);
        TurnOffSpotLightsOnChildren(cardInstance);
        Card cardScript = cardInstance.GetComponent<Card>();
        SetCardMaterial(cardData, cardScript);
        TurnOffScriptsOnCard(cardInstance);

        return cardInstance;
    }

    private void SetCardMaterial(CardData cardData, Card cardScript)
    {

        MeshRenderer frontMesh = cardScript.CardFront.GetComponent<MeshRenderer>();
        MeshRenderer backMesh = cardScript.CardBack.GetComponent<MeshRenderer>();
        frontMesh.material = cardData.frontMaterial;
        backMesh.material = cardData.backMaterial;

    }

    private void TurnOffScriptsOnCard(GameObject cardPrefab)
    {
        ObjectRotation objectRotation = cardPrefab.GetComponent<ObjectRotation>();
        objectRotation.enabled = false;
        Loot_Hover hover = cardPrefab.GetComponent<Loot_Hover>();
        hover.enabled = false;
        BoxCollider collider = cardPrefab.GetComponent<BoxCollider>();
        collider.enabled = false;
        BounceScript bounce = cardPrefab.GetComponent<BounceScript>();
        bounce.enabled = false;
        Rigidbody body = cardPrefab.GetComponent<Rigidbody>();
        body.useGravity = false;
        Light light = cardPrefab.GetComponentInChildren<Light>();
        light.enabled = false;
        //ParticleSystem particleSystem = cardPrefab.GetComponentInChildren<ParticleSystem>();
        //particleSystem.Stop();
    }

    private void TurnOnScriptsOnCard(GameObject cardPrefab)
    {
        ObjectRotation objectRotation = cardPrefab.GetComponent<ObjectRotation>();
        objectRotation.enabled = false;
        Loot_Hover hover = cardPrefab.GetComponent<Loot_Hover>();
        hover.enabled = true;
        BoxCollider collider = cardPrefab.GetComponent<BoxCollider>();
        collider.enabled = true;
        BounceScript bounce = cardPrefab.GetComponent<BounceScript>();
        bounce.enabled = true;
        Rigidbody body = cardPrefab.GetComponent<Rigidbody>();
        body.useGravity = true;
    }

    public void InstatitateCard(CardData card, Vector3 spawnPos)
    {
        GameObject spawnedCard = Instantiate(cardPrefab, spawnPos + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(ReturnNormalParticles(LootManager.instance.GetRarityFromTier(card.cardTier)), spawnedCard.transform.position, Quaternion.identity, spawnedCard.transform);
        Card script = spawnedCard.GetComponent<Card>();
        script.SetCardData(card);
        SetCardMaterial(card, script);
        TurnOnScriptsOnCard(spawnedCard);
    }
    /// <summary>
    /// Retrieves the particle system associated with the specified rarity tier.
    /// </summary>
    /// <param name="rarityTier">The rarity tier for which to obtain the corresponding particle system.</param>
    /// <returns>The particle system that corresponds to the specified rarity tier. If the rarity tier is not recognized, the
    /// particle system for the Common tier is returned.</returns>
    private ParticleSystem ReturnNormalParticles(RarityTier rarityTier)
    {
        switch (rarityTier)
        { 
            case RarityTier.Common:
                return normalDropParticles[0];
            case RarityTier.Uncommon:
                return normalDropParticles[1];
            case RarityTier.Rare:
                return normalDropParticles[2];
            case RarityTier.Epic:
                return normalDropParticles[3];
            case RarityTier.Legendary:
                return normalDropParticles[4];
            default: return normalDropParticles[0];
        }
    }

    private ParticleSystem ReturnParticleShopboard(RarityTier rarityTier)
    {
        switch (rarityTier)
        {
            case RarityTier.Common:
                return shopBoardParticles[0];
            case RarityTier.Uncommon:
                return shopBoardParticles[1];
            case RarityTier.Rare:
                return shopBoardParticles[2];
            case RarityTier.Epic:
                return shopBoardParticles[3];
            case RarityTier.Legendary:
                return shopBoardParticles[4];
            default: return shopBoardParticles[0];
        }
    }
    /// <summary>
    /// Retrieves all cards with a tier of Rare, Epic, or Legendary.
    /// </summary>
    /// <remarks>Use this method to obtain cards that are above or equal to the Rare tier for scenarios where
    /// only higher-tier cards are relevant, such as special rewards or advanced gameplay features.</remarks>
    /// <returns>A list of <see cref="CardData"/> objects representing cards whose tier is Rare, Epic, or Legendary. The list
    /// will be empty if no such cards are found.</returns>
    public List<CardData> ReturnAboveRareTier()
    {
        LootManager lootManager = LootManager.instance;

        var tempList = cardSystem.GetAllCards();
        List<CardData> result = new List<CardData>();

        foreach (CardData card in tempList) // Checks if current card's tier is within the Rare, Epic, Legendary range.
        {
            if (lootManager.IsTierInsideRarity(card.cardTier, RarityTier.Rare)
               || lootManager.IsTierInsideRarity(card.cardTier, RarityTier.Epic)
               || lootManager.IsTierInsideRarity(card.cardTier, RarityTier.Legendary))
            {
                result.Add(card);
            }
        }
        return result;
    }

    private void TurnOffSpotLightsOnChildren(GameObject obj)
    {
        foreach (Light spotlight in obj.GetComponentsInChildren<Light>())
        {
            spotlight.enabled = false;
        }
    }






}
