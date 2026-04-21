using UnityEngine;

public class ObjectHoverController : MonoBehaviour
{
    private Transform hover;

    public float speed = 1f;
    public float amplitude = 0.5f;
    public float frequency = 2f;

    private float time;

    void Start()
    {
        hover = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        float hover = Mathf.Sin(time * frequency) * amplitude;

        transform.position += new Vector3(0f, hover, 0f);
    }
}
