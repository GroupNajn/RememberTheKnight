using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGUI;

public class PlayerCollection : MonoBehaviour
{
  
        CardCollection cardCollection;

    // A class to hold different collections that belongs to the player.
    
    
    void Start()
    {
        cardCollection = new CardCollection();
    }




   
    void Update()
    {
        
    }
}
