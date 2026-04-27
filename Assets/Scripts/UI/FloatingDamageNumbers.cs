using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UIElements;


// Script made by Henric 2026-03-25
public class FloatingDamageNumbers : MonoBehaviour
{

    [SerializeField] GameObject prefab;
    private Transform spawnPos;
    private TextMeshProUGUI textMesh;
   

    private void Awake()
    {

        //spawnPos = transform.Find("HealthBar/Numbers_Spawn_Position");
        //Debug.LogError(spawnPos);
    }

    private void OnDisable()
    {
        Event_System.instance.OnEnemyDamage -= SpawnFloatingNumbers;
    }

    private void Start()
    {

        spawnPos = transform.Find("EnemyHealthBar/Numbers_Spawn_Position");
        Event_System.instance.OnEnemyDamage += SpawnFloatingNumbers;
    }

    private void Update()
    {
        if (textMesh != null)
            textMesh.transform.rotation = this.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
       

    }

    IEnumerator DisableAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }


    private void SpawnFloatingNumbers(Transform parentTransform, float damage)
    {

        if (parentTransform != transform.parent)
            return;
        //var transform = GetComponent<Transform>().Find("HealthBar");
        var popup = Instantiate(prefab, spawnPos.position, Quaternion.identity, transform);
       
        textMesh = popup.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = damage.ToString();
        
        StartCoroutine(DisableAfterTime(popup, 3f));
    }
}