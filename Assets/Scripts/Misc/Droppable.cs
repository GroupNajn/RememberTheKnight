using UnityEngine;

public class Droppable : MonoBehaviour
{

    [SerializeField] public float Weight = 5.0f;
     
    [SerializeField] string Name = "Default";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {

    }
    private void OnEnable()
    {
        LootManager.instance.RegisterLoot(this);
    }

    private void OnDisable()
    {
        LootManager.instance.UnregisterLoot(this);
    }
}
