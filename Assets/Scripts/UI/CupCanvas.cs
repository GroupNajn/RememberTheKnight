using UnityEngine;

public class CupCanvas : MonoBehaviour
{
    GameObject fullHealingCup;
    GameObject emptyHealingCup;

    void Start()
    { 
        fullHealingCup = transform.Find("HealingCup").Find("Full").gameObject;
        emptyHealingCup = transform.Find("HealingCup").Find("Empty").gameObject;

        fullHealingCup.SetActive(false);

        gameObject.SetActive(false);
    }

    public void FillealingCup()
    {
        fullHealingCup.SetActive(true);
        emptyHealingCup.SetActive(false);
    }

    public void EmptyHealingCup()
    {
        fullHealingCup.SetActive(false);
        emptyHealingCup.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FillealingCup();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            EmptyHealingCup();
        }
    }
}