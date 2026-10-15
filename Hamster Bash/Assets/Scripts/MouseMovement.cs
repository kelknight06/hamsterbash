using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock cursor to middle screen and make it invisible.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Looking up and down
        xRotation -= mouseY;

        // Clamp rotation
        xRotation = Mathf.Clamp (xRotation, topClamp, bottomClamp);

        // Look left and right
        yRotation += mouseX;

        // Apply rotations to transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
