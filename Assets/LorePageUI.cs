
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
public class LorePageUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform lorePageWindowRect;
    [SerializeField] private float duration = 1f;
    [SerializeField] AnimationCurve bounceCurve;
    [SerializeField] List<GameObject> gameObjectsToDisable;
    [SerializeField] TextMeshProUGUI loreTextTMP;

    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BookUi book;
    [SerializeField] private PlayerStats playerStats;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void OnEnable()
    {
        DisableGameObjects();
        StartCoroutine(ScaleBouncePickUpWindow());
    }

    private IEnumerator ScaleBouncePickUpWindow()
    {
        float timer = 0f;
        // 300 x 450
        // 300 x 1.5
        // 350 x 525
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / duration;
            float curvevalue = bounceCurve.Evaluate(t);

            lorePageWindowRect.transform.localScale = Vector3.one * curvevalue;

            yield return null;
        }
        lorePageWindowRect.transform.localScale = Vector3.one;
        //LeanTween.scale(rescaleRect, new Vector3(0.95f, 0.95f, 0.95f), 1.5f).setEaseInBack().setLoopPingPong().setIgnoreTimeScale(true);
        EnableGameObjects();
    }


    public void OnRememberLore()
    {
        uiManager.CloseLorePageUI();
        uiManager.OpenBookUI();

        book.OpenTab(BookUi.BookTabEnum.Lore);
    }

    void EnableGameObjects()
    {
        foreach(GameObject go in gameObjectsToDisable)
        {
            go.SetActive(true);
        }
    }

    public void SetLoreEntry(LoreEntry entry)
    {
        loreTextTMP.text = entry.GetPages(500)[0];
    }

    void DisableGameObjects()
    {

        foreach(GameObject go in gameObjectsToDisable)
        {
            go.SetActive(false);
        }

    }
}
