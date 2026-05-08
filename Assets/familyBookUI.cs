using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class familyBookUI : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject swordsMarkerRight;
    [SerializeField] private GameObject wandsMarkerRight;
    [SerializeField] private GameObject cupsMarkerRight;
    [SerializeField] private GameObject pentaclesMarkerRight;

    [SerializeField] private GameObject swordsMarkerLeft;
    [SerializeField] private GameObject wandsMarkerLeft;
    [SerializeField] private GameObject cupsMarkerLeft;
    [SerializeField] private GameObject pentaclesMarkerLeft;



    public void Setup(CardFamily family, CardData cardData)
    {

        swordsMarkerRight.SetActive(false);
        wandsMarkerRight.SetActive(false);
        cupsMarkerRight.SetActive(false);
        pentaclesMarkerRight.SetActive(false);

        swordsMarkerLeft.SetActive(false);
        wandsMarkerLeft.SetActive(false);
        cupsMarkerLeft.SetActive(false);
        pentaclesMarkerLeft.SetActive(false);

        switch (family)
        {
            case CardFamily.Swords:
                wandsMarkerRight.SetActive(true); //the rest of the book marks true based on what side it should be on
                cupsMarkerRight.SetActive(true);
                pentaclesMarkerRight.SetActive(true);

                ShowInfo(cardData); // show the info about this family
                break;

            case CardFamily.Wands:

                swordsMarkerLeft.SetActive(true);
                cupsMarkerRight.SetActive(true);
                pentaclesMarkerRight.SetActive(true);

                ShowInfo(cardData);
                break;

            case CardFamily.Cups:

                swordsMarkerLeft.SetActive(true);
                wandsMarkerLeft.SetActive(true);
                pentaclesMarkerRight.SetActive(true);

                ShowInfo(cardData);
                break;

            case CardFamily.Pentacles:

                swordsMarkerLeft.SetActive(true);
                wandsMarkerLeft.SetActive(true);
                cupsMarkerLeft.SetActive(true);

                ShowInfo(cardData);
                break;
        }
    }

    private void ShowInfo(CardData cardData)
    {
       // SHOW INFO TEXT

        if (cardData == null) return;

        cardData.cardFamily.ToString();
    }



}
