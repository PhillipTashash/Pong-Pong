using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallMovement : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 1f;
    //Movement speed we will multiple by the .normalized linear velocity script.
    private Vector2 startMoveVector = new Vector2(1f, 1f);
    //Set the initial movement vector for the ball.
    Rigidbody2D rb;
    //Create the rigidbody component for the ball.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Initialize the rigidbody component.
        rb.linearVelocity = startMoveVector.normalized * moveSpeed;
        //Set the ball to start moving in the direction of startMoveVector.
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 v = rb.linearVelocity;
        //Get the current linear velocity of the ball.

        if (col.gameObject.CompareTag("Paddle")) //If the ball collides with the paddle.
        {
            // Handle collision with paddle.
            float offset = (transform.position.y - col.transform.position.y) / col.collider.bounds.size.y;
            //
            float dirX = -Mathf.Sign(v.x);
            rb.linearVelocity = new Vector2(dirX, offset).normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = new Vector2(v.x, -v.y).normalized * moveSpeed;
        }
    }
}
