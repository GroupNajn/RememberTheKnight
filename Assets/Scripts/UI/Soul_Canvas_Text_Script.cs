using UnityEngine;
using TMPro;
public class Soul_Canvas_Text_Script : MonoBehaviour
{
    int soulAmount = 0;
    TextMeshProUGUI tmp;

    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = soulAmount.ToString();
    }

    public void SetSoulsAmount(int souls)
    {
       
        tmp.text = souls.ToString();
      
    }



    

}
