using UnityEngine;
using TMPro;
public class Soul_Canvas_Text_Script : MonoBehaviour
{
    int soulAmount = 0;
    TextMeshProUGUI tmp;
    private bool canIncreaseSouls = true;

    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = soulAmount.ToString();
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void OnDisable()
    {
       
    }

    public void SetSoulsAmount(int souls)
    {
       
        tmp.text = souls.ToString();
      
    }



    

}
