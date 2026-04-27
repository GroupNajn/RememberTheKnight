using UnityEngine;
using TMPro;

public class CupCanvas : MonoBehaviour
{
    public static CupCanvas Instance { get; private set; }

    [SerializeField] Gradient cupGradient;
    [SerializeField] Gradient crystalGradient;
    [SerializeField] Material cupMaterial;
    [SerializeField] TMP_Text usesText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    { 
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

        if (chargesLeft == maxCharges)
        {
            cupMaterial.SetColor("_EmissionColor", crystalGradient.Evaluate(1) * 2f);
        }
        else
        {
            cupMaterial.SetColor("_EmissionColor", crystalGradient.Evaluate(crystalFillAmount) * 2f);
        }

        usesText.text = $"{usesLeft}";
    }
}