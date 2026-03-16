using UnityEngine;

public class Droppable : MonoBehaviour
{

   protected float weight { get; set; }
    public float Weight => weight;
    [SerializeField] private LootManager LootManager;
    public Vector3 position;
    Transform transform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weight = 5.0f;
        transform = GetComponent<Transform>();
        position = transform.position;
    }

    void Update()
    {

    }
    private void OnEnable()
    {
        LootManager.RegisterLoot(this);
    }

    private void OnDisable()
    {
        LootManager.UnregisterLoot(this);
    }
}
