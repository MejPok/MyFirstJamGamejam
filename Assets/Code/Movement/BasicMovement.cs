using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ReturnVine))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PositionTracker))]
public class BasicMovement : MonoBehaviour
{
    public float Speed = 1;
    public float MaxSpeed = 2;

    Rigidbody2D rb;

    public Vector2 MousePosition;

    Wallchecker wallcheck;
    public PositionTracker posTracker;


    /// <summary>
    /// Updates the world-space mouse position each frame.
    /// </summary>
    void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    Vector2 moveDirection;
    public float MaxDistance = 5f; // Maximum allowed distance from last saved position

    public float DistanceWhileNotTouchingWall { get; set; }
    private Vector2 lastPosition;
    ReturnVine returnVine;

    void Start()
    {
        returnVine = GetComponent<ReturnVine>();
        rb = GetComponent<Rigidbody2D>();
        posTracker = GetComponent<PositionTracker>();
        wallcheck = transform.GetChild(0).GetComponent<Wallchecker>();

        lastPosition = rb.position;
    }

    

    void FixedUpdate()
    {
        moveDirection = MousePosition - (Vector2)transform.position;

        if (Mouse.current.leftButton.isPressed && allowedToMoveInsideBoundary && !returnVine.returningVine)
        {
            rb.AddForce(moveDirection * Speed);
            PlaySound();
        }

        // Clamp velocity to MaxSpeed to prevent exceeding the speed limit
        if (rb.linearVelocity.magnitude > MaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * MaxSpeed;
        }
        
        DeleteSmallSpeed();
        CheckMaxDistance();
    }


    /// <summary>
    /// Stops the rigidbody if it's moving below a small velocity threshold.
    /// </summary>
    void DeleteSmallSpeed()
    {
        if (rb.linearVelocity.magnitude < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            StopSound();
        }
    }

    /// <summary>
    /// Tracks distance traveled while not touching a wall, and stops movement if limits are exceeded.
    /// </summary>
    void CheckMaxDistance()
    {

        Vector2 currentPosition = Vector2.zero;
        if (returnVine.returningVine)
        {
            currentPosition = rb.position;
            lastPosition = currentPosition;
            return;
        }

        currentPosition = rb.position;

        float delta = Vector2.Distance(currentPosition, lastPosition);
        if (!wallcheck.touchingWall)
        {
            DistanceWhileNotTouchingWall += delta;
        } else {
            DistanceWhileNotTouchingWall = 0;
        }

        lastPosition = currentPosition;

        if (DistanceWhileNotTouchingWall > MaxDistance)
        {
            Debug.Log("I cant move, i am too far from a wall, ");
            rb.linearVelocity = Vector2.zero;
            allowedToMoveInsideBoundary = false;
        } else {
            allowedToMoveInsideBoundary = true;
        }
    }
    bool allowedToMoveInsideBoundary;

    GameObject sound;
    public void PlaySound()
    {
        if(sound == null)
        {
            sound = Instantiate(SoundManager.instance.audioPrefab, transform.position, Quaternion.identity);
            sound.GetComponent<AudioSource>().clip = GetComponent<SoundHolder>().holder[0];
            sound.GetComponent<AudioSource>().volume = 1;
            sound.GetComponent<AudioSource>().loop = true;
        }
        
        sound.GetComponent<AudioSource>().Play();
    }
    public void StopSound()
    {
        if(sound != null)
        {
            sound.GetComponent<AudioSource>().Stop();
            
        }
    }
}
