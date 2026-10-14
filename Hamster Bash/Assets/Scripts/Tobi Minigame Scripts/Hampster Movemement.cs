using UnityEngine;

public class HampsterMovemement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float sideSpeed = 5f;

    void Update()
    {
        // Automatically move forward
        transform.Translate(
            Vector3.forward * forwardSpeed * Time.deltaTime,
            Space.World
        );

        // Move left and right using A/D or arrow keys
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(
            Vector3.right * horizontalInput * sideSpeed * Time.deltaTime,
            Space.World
        );
    }
}
