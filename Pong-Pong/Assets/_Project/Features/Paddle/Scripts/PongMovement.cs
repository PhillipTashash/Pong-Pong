using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PongMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveAction;
    Camera cam;
    private float halfPaddle;

    void OnEnable() => moveAction.action.Enable();
    void OnDisable() => moveAction.action.Disable();

    void Start()
    {
        cam = Camera.main;
        halfPaddle = GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Update()
    {
        float move = moveAction.action.ReadValue<float>();

        float top = cam.transform.position.y + cam.orthographicSize - halfPaddle;
        float bottom = cam.transform.position.y - cam.orthographicSize + halfPaddle;

        transform.position += Vector3.up * move * moveSpeed * Time.deltaTime;

        // Keep the paddle within the camera bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottom, top), transform.position.z);
    }
}
