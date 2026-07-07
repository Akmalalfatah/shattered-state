using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform player;

    public float parallaxAmount = 0.2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newX = startPosition.x + (player.position.x * parallaxAmount);

        transform.position = new Vector3(
            newX,
            startPosition.y,
            startPosition.z
        );
    }
}