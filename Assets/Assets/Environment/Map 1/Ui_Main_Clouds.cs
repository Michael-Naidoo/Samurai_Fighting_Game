using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class CloudMovement : MonoBehaviour
{
    public float speed = 10f; // Adjust speed as needed
    public float minX = -1000f; // Left boundary for reset
    public float maxX = 1000f; // Right boundary for reset

    void Update()
    {
        // Move the cloud horizontally
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // If the cloud moves past the right boundary, reset it to the left
        if (transform.position.x > maxX)
        {
            Vector2 newPos = new Vector2(minX, transform.position.y);
            transform.position = newPos;
        }
    }
}