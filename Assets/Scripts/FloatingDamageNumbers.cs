using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UIElements;

public class FloatingDamageNumbers : MonoBehaviour
{


    [SerializeField] GameObject prefab;
    private Transform spawnPos;
    private TextMeshProUGUI textMesh;
   

    private void Awake()
    {


    }

    private void OnDisable()
    {
        Event_System.instance.OnEnemyDamage -= SpawnFloatingNumbers;
    }

    private void Start()
    {
        spawnPos = transform.GetChild(3);
        Event_System.instance.OnEnemyDamage += SpawnFloatingNumbers;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    SpawnFloatingNumbers();
        //}
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
       
        if (parentTransform.root != this.transform.root) return;

        var popup = Instantiate(prefab, spawnPos.position, Quaternion.identity, spawnPos);
        
        textMesh = popup.GetComponent<TextMeshProUGUI>();
        textMesh.text = damage.ToString();
        
        StartCoroutine(DisableAfterTime(popup, 1f));
    }
}