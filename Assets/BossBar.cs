using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] EnemyDamage enemyDamage;

    [SerializeField] Slider bossBar;

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


}
