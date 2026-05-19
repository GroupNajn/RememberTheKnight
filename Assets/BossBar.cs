using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] EnemyDamage enemyDamage;

    [SerializeField] Slider bossBar;

    [SerializeField] float lerpSpeed = 2f;
    public RectTransform lerpingRectTransform;
    bool barSetupComplete = false;

    private void Start()
    {
        Debug.Log("Start called in BossBar");
        Event_System.instance.OnSpawnBoss += ShowBossBar;
        Event_System.instance.OnBossDeath += CloseBossBar;
        Debug.Log("Successfully subscribed to events");
    }

    public void BarSetup()
    {
        Debug.Log("Bar setup");
        Boss = GameObject.FindGameObjectWithTag("Boss");
        enemyDamage = Boss.GetComponent<EnemyDamage>();
        Debug.Log("Stuff found");

        bossBar.maxValue = enemyDamage.MaxHealth;
        bossBar.value = enemyDamage.Health;

        enemyDamage.OnHealthChanged += OnHealthChanged;
        barSetupComplete = true;
    }

    public void ShowBossBar()
    {
        Debug.Log("Show boss bar");
        BarSetup();
        bossBar.gameObject.SetActive(true);
    }

    public void CloseBossBar()
    {
        bossBar.gameObject.SetActive(false);
    }

    public void OnHealthChanged(float current, float max)
    {
        bossBar.value = current;
    }

    private void Update() // Lerp the healthbar fill to the target position
    {
        if (barSetupComplete)
        {
            bool lerpCondition = lerpingRectTransform.anchorMax.x > bossBar.fillRect.anchorMax.x || lerpingRectTransform.anchorMin.x < bossBar.fillRect.anchorMin.x;

            if (lerpCondition)
            {
                lerpingRectTransform.anchorMax = Vector2.Lerp(lerpingRectTransform.anchorMax, bossBar.fillRect.anchorMax, Time.deltaTime * lerpSpeed);
                lerpingRectTransform.anchorMin = Vector2.Lerp(lerpingRectTransform.anchorMin, bossBar.fillRect.anchorMin, Time.deltaTime * lerpSpeed);
            }
            else
            {
                lerpingRectTransform.anchorMax = bossBar.fillRect.anchorMax;
                lerpingRectTransform.anchorMin = bossBar.fillRect.anchorMin;
            }
        }

    }
}
