using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;
    public Vector2 moveInput;
    public float speed = 10.0f;

    private void Start()
    {
        moveAction.Enable();
        
    }
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
        transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * speed);
    }
}