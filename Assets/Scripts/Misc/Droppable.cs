using UnityEngine;

public class Droppable : MonoBehaviour
{

    [SerializeField] public float Weight = 5.0f;
     
    [SerializeField] string Name = "Default";
   
    public Vector3 position;
    Transform transform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform = GetComponent<Transform>();
        position = transform.position;
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
