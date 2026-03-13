using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    public Vector2 move;

    public float Speed = 1;
    public float BackwardSpeed = 2;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
    }

    public float speedLimit;
    void FixedUpdate()
    {
        move = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();

        if(move.x > 0)
        {
            if(rb.linearVelocityX < speedLimit)
            {
                rb.AddForceX(move.x * Speed);    
            } else
            {
                rb.linearVelocity = new Vector2(speedLimit, rb.linearVelocityY);
            }
        } else
        {
            if(rb.linearVelocityX > -speedLimit)
            {
                rb.AddForceX(move.x * Speed);    
            } else
            {
                rb.linearVelocity = new Vector2(-speedLimit, rb.linearVelocityY);
            }
        }

        if(move.y > 0)
        {
            if(rb.linearVelocityY < speedLimit)
            {
                rb.AddForceY(move.y * Speed);    
            } else
            {
                rb.linearVelocity = new Vector2(speedLimit, rb.linearVelocityY);
            }
        } else
        {
            if(rb.linearVelocityY > -speedLimit)
            {
                rb.AddForceY(move.y * Speed);    
            } else
            {
                rb.linearVelocity = new Vector2(-speedLimit, rb.linearVelocityY);
            }
        }
    }

}
