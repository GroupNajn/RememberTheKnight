
using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static LootManager instance;
    [SerializeField] List<GameObject> droppedItems = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
