using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallMovement : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)]private float moveSpeed = 1f;
    [SerializeField, Range(-10f, 10f)]private float startVectorX = 1f;
    [SerializeField, Range(-10f, 10f)]private float startVectorY = 0f;
    [SerializeField, Range(1f, 2f)]private float speedAddWhenHitPaddle = 1.1f;
    //Movement speed we will multiple by the .normalized linear velocity script.
    
    //Set the initial movement vector for the ball.
    Rigidbody2D rb;
    //Create the rigidbody component for the ball.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Initialize the rigidbody component.
        rb.linearVelocity = new Vector2(startVectorX, startVectorY).normalized * moveSpeed;
        //Set the ball to start moving in the direction of startMoveVector.
    }

    void OnCollisionEnter2D(Collision2D col) //Ball colliding with something.
    {
        Vector2 v = rb.linearVelocity;
        //Get the current linear velocity of the ball.

        if (col.gameObject.CompareTag("Paddle")) // P1 and P2 paddles.
        {
            // Handle collision with paddle.
            float offset = (transform.position.y - col.transform.position.y) / col.collider.bounds.size.y;
            // By getting the difference between the balls position (y) and the paddle's position (y) you can
            // determine where on the paddle the ball hits. Then if you divide the collision point by
            //  the paddle's height, you get a value between -1 and 1.
            float dirX = Mathf.Sign(transform.position.x - col.transform.position.x);
            // Send the ball away from the paddle it touched.
            rb.linearVelocity = new Vector2(dirX, offset).normalized * moveSpeed;
            moveSpeed *= speedAddWhenHitPaddle;
        }
        else if (col.gameObject.CompareTag("Wall")) //Top and bottom walls.
        {
            float dirY = Mathf.Sign(transform.position.y - col.transform.position.y);
            // Determine the direction the ball should bounce based on the wall it hit.
            rb.linearVelocity = new Vector2(v.x, dirY).normalized * moveSpeed;
            // Change the direction the ball bounces.
        }
    }
}
