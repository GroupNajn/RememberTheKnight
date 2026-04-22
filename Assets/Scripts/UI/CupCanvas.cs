using UnityEngine;

public class CupCanvas : MonoBehaviour
{
    [SerializeField] Gradient cupGradient;
    [SerializeField] Gradient crystalGradient;
    [SerializeField] Material cupMaterial;

    GameObject fullHealingCup;
    GameObject emptyHealingCup;

    int chargesLeft;

    void Start()
    { 
        fullHealingCup = transform.Find("HealingCup").Find("Full").gameObject;
        emptyHealingCup = transform.Find("HealingCup").Find("Empty").gameObject;

        fullHealingCup.SetActive(false);

        gameObject.SetActive(false);
    }

    public void UpdateCup(int chargesLeft, int maxCharges, int chargesPerUse)
    {
        int usesLeft = chargesLeft / chargesPerUse;
        int chargestToNextUse = chargesLeft % chargesPerUse;
        int maxUses = maxCharges / chargesPerUse;

        float cupFillAmount = (float)usesLeft / maxUses;
        float crystalFillAmount = (float)chargestToNextUse / chargesPerUse;

        cupMaterial.SetColor("_BaseColor", cupGradient.Evaluate(cupFillAmount));
        cupMaterial.SetColor("_EmissionColor", crystalGradient.Evaluate(crystalFillAmount) * 2f);
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
            if (chargesLeft >= 0 && chargesLeft < 100)
            {
                chargesLeft++;
                UpdateCup(chargesLeft, 100, 10);
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (chargesLeft > 0 && chargesLeft <= 100)
            {
                chargesLeft--;
                UpdateCup(chargesLeft, 100, 10);
            }
        }
    }
}