using UnityEngine;
using UnityEngine.InputSystem;

public class PongMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private Collider2D topBound;
    [SerializeField] private Collider2D bottomBound;
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

        float top = topBound.bounds.min.y - halfPaddle;
        float bottom = bottomBound.bounds.max.y + halfPaddle;

        transform.position += Vector3.up * move * moveSpeed * Time.deltaTime;

        // Keep the paddle within the camera bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottom, top), transform.position.z);
    }
}
