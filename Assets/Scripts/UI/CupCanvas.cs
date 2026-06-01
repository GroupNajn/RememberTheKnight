using UnityEngine;
using TMPro;

public class CupCanvas : MonoBehaviour
{
    /// <summary>
    /// Made by Lukas 2026-04-20
    /// Swapping between a filled and empty cup depending on if it's full or empty
    /// 
    /// Updated by Lukas 2026-05-22
    /// Updates the healing cup colors with a gradient based depending on the current number of charges left, the maximum charges, and the charges per use
    /// </summary>

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

    private void OnDestroy()
    {
        UpdateCup(0, 100, 10);
    }
}