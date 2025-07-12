using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    Vector3 initialPosition;
    [SerializeField] float speed = 5f;
    [SerializeField] float oscilation = 5f;
    float amplitude;
    [SerializeField] Vector3 movementDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        amplitude = Mathf.SmoothStep(-oscilation, oscilation, Mathf.PingPong(Time.time * speed, 1f));

        transform.position = initialPosition + movementDirection * amplitude;
    }
}
