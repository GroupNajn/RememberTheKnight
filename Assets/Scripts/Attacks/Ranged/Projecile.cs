using UnityEngine;

public class Projecile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool colided;

    public float speed;
    public Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "projectile" && other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
