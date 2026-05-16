using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider bossHealth;

    [SerializeField] private GameObject bossObject;
    [SerializeField] private EnemyDamage enemyDamage;

    [SerializeField] bool isActive;

    public void Start()
    {
        Event_System.instance.OnSpawnBoss += () => SetActive(true);
        Event_System.instance.OnBossDeath += () => SetActive(false);
    }

    public void SetBarMax()
    {
        bossHealth.maxValue = enemyDamage.MaxHealth;
        bossHealth.value = enemyDamage.Health;
    }

    public void OnHealthChanged()
    {
         bossHealth.value = enemyDamage.Health;
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            if(bossObject == null)
            {
                bossObject = GameObject.FindGameObjectWithTag("Boss");
                enemyDamage = bossObject.GetComponent<EnemyDamage>();

                SetBarMax();

                bossHealth.gameObject.SetActive(true);
            }
        }
        else
        {
            bossHealth.gameObject.SetActive(false);
        }
    }

}

