using System;
using UnityEngine;

public class CardBuilder : MonoBehaviour
{
    private Transform parentSpawnPosTrans;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Card cardScript;
    [SerializeField] private CardSystem cardSystem;
    [SerializeField] Transform playerTrans;
    bool keyPressed = false;
    void Start()
    {

    }

    void Update()
    {

        //if (Input.GetKey(KeyCode.P))
        //{
        //    if (!keyPressed)
        //    {
        //        parentSpawnPosTrans = playerTrans;
        //        InstantiateCardWithoutScripts(cardSystem.ReturnRandomCard());
        //    }
        //    keyPressed = true;
        //}

        //keyPressed = false;
    }


    public GameObject InstantiateCardWithoutScripts(CardData cardData, Transform parentTransform)
    {
        Quaternion rotation = Quaternion.Euler(parentTransform.rotation.eulerAngles.x, parentTransform.rotation.eulerAngles.y + 180, parentTransform.rotation.eulerAngles.z);

        GameObject cardInstance = Instantiate(cardPrefab, parentTransform.position, rotation, parentTransform);
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

    }

    public void InstatitateCard(CardData card, Vector3 spawnPos)
    {
        GameObject spawnedCard = Instantiate(cardPrefab, spawnPos, Quaternion.identity);


    }




}
