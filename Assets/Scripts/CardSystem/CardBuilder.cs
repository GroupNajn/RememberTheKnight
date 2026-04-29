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

        if (Input.GetKey(KeyCode.P))
        {
            if (!keyPressed)
            {
                parentSpawnPosTrans = playerTrans;
                InstantiateCardWithoutScripts(cardSystem.ReturnRandomCard());
            }
            keyPressed = true;
        }

        keyPressed = false;
    }


    public GameObject InstantiateCardWithoutScripts(CardData cardData)
    {

        Instantiate(cardPrefab, parentSpawnPosTrans.position, Quaternion.identity);
        SetCardMaterial(cardData, cardScript);
        TurnOffScriptsOnCard(cardPrefab);

        return cardPrefab;
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




}
