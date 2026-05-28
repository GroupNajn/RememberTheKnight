using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameDataUpdater : MonoBehaviour
{
    private GameData gameData;
    private RunGameData runGameData;
    public static GameDataUpdater instance;

    public Dictionary<string, int> donatedSinceLastDic;
    public Dictionary<string, int> soulsRemainingTillNext;

    void Start()
    {
        gameData = GetComponent<GameData>();
        runGameData = GetComponent<RunGameData>();
        InitializeDictonaries();
        StartCoroutine(AutoSave());
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate manager destroyed");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void InitializeDictonaries()
    {
        donatedSinceLastDic = new Dictionary<string, int>
        {
            { "soulsDonatedSinceLastCups", gameData.soulsDonatedSinceLastCups },
            { "soulsDonatedSinceLastWands", gameData.soulsDonatedSinceLastWands },
            { "soulsDonatedSinceLastPentacles", gameData.soulsDonatedSinceLastPentacles },
            { "soulsDonatedSinceLastSwords", gameData.soulsDonatedSinceLastSwords }
         };
        soulsRemainingTillNext = new Dictionary<string, int>
        {
            {"SoulsRemainingsoulToNextUnlockCups", gameData.SoulsRemainingsoulToNextUnlockCups },
            {"SoulsRemainingToNextUnlockWands", gameData.SoulsRemainingToNextUnlockWands },
            {"SoulsRemainingToNextUnlockPentacles", gameData.SoulsRemainingToNextUnlockPentacles },
            {"SoulsRemainingToNextUnlockSwords", gameData.SoulsRemainingToNextUnlockSwords }
        };

    }

    public static void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
    }

    // (Next card in family) - currentSoulsDonated;

    private float CalculateNextCardCost => Mathf.Pow(GameObject.Find("CardSystem").GetComponent<CardSystem>().
                                           GetNextCardInSelectedFamily().cardSoulCost, 2);

    public void SetSoulsCostInNextCard(CardContract contract)
    {
        switch (contract.CardFamily)
        {
            case CardFamily.Cups:
                gameData.SoulsRemainingsoulToNextUnlockCups = (int)CalculateNextCardCost;
                break;
            case CardFamily.Wands:
                gameData.SoulsRemainingToNextUnlockWands = (int)CalculateNextCardCost;
                break;
            case CardFamily.Pentacles:
                gameData.SoulsRemainingToNextUnlockPentacles = (int)CalculateNextCardCost;
                break;
            case CardFamily.Swords:
                gameData.SoulsRemainingToNextUnlockSwords = (int)CalculateNextCardCost;
                break;
        }
    }

    public void SetSoulsDonatedSinceLastToFamily(CardContract contract, int amount)
    {
        switch (contract.CardFamily)
        {
            case CardFamily.Cups:
                gameData.soulsDonatedSinceLastCups = amount;
                break;
            case CardFamily.Wands:
                gameData.soulsDonatedSinceLastWands = amount;
                break;
            case CardFamily.Pentacles:
                gameData.soulsDonatedSinceLastPentacles = amount;
                break;
            case CardFamily.Swords:
                gameData.soulsDonatedSinceLastSwords = amount;
                break;
        }
    }

    public int GetSoulCostForIngameRun(CardContract contract)
    {
        switch (contract.CardFamily)
        {
            case CardFamily.Cups:
                return gameData.SoulsRemainingsoulToNextUnlockCups;

            case CardFamily.Wands:
                return gameData.SoulsRemainingToNextUnlockWands;

            case CardFamily.Pentacles:
                return gameData.SoulsRemainingToNextUnlockPentacles;

            case CardFamily.Swords:
                return gameData.SoulsRemainingToNextUnlockSwords;
            default: return 0;
        }
    }

    public void SetSoulsRemaingToNextUnlock(CardContract contract, int amount)
    {

        switch (contract.CardFamily)
        {
            case CardFamily.Cups:
                gameData.SoulsRemainingsoulToNextUnlockCups -= amount;
                break;

            case CardFamily.Wands:
                gameData.SoulsRemainingToNextUnlockWands -= amount;
                break;

            case CardFamily.Pentacles:
                gameData.SoulsRemainingToNextUnlockPentacles -= amount;
                break;

            case CardFamily.Swords:
                gameData.SoulsRemainingToNextUnlockSwords -= amount;
                break;
        }
    }

    public void SaveDonatedSinceLastToPlayerPrefs()
    {
        foreach (KeyValuePair<string, int> pair in donatedSinceLastDic)
        {
            PlayerPrefsSaveSystem.SaveSouls(pair.Key, pair.Value);
        }
    }

    public void SaveSoulsRemainingToPlayerPrefs()
    {
        foreach (KeyValuePair<string, int> pair in soulsRemainingTillNext)
        {
            PlayerPrefsSaveSystem.SaveSouls(pair.Key, pair.Value);
        }

    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(30);
            InitializeDictonaries();
            SaveDonatedSinceLastToPlayerPrefs();
            SaveSoulsRemainingToPlayerPrefs();
        }
    }



}