using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    /// <summary>
    /// Movement speed in units per second.
    /// Increase this value if movement feels too slow (1 is intentionally low).
    /// </summary>
    public float Speed = 5f;

    private Rigidbody2D rb;

    /// <summary>
    /// Current mouse position in world space.
    /// </summary>
    public Vector2 MousePosition;

    private Wallchecker wallcheck;
    private PositionTracker posTracker;

    /// <summary>
    /// Maximum allowed distance from the last saved position.
    /// The player can move freely inside this circle, but cannot go outside.
    /// </summary>
    public float MaxDistance = 5f;

    /// <summary>
    /// Total distance traveled while not touching a wall. Used for tracking purposes.
    /// </summary>
    public float DistanceWhileNotTouchingWall { get; private set; }

    private Vector2 lastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posTracker = GetComponent<PositionTracker>();
        wallcheck = transform.GetChild(0).GetComponent<Wallchecker>();

        // Initialize tracking
        lastPosition = rb.position;
    }

    void Update()
    {
        // Keep the world-space mouse position updated every frame.
        MousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    void FixedUpdate()
    {
        // Apply movement and keep the player within the allowed radius.
        ApplyMovementWithinBounds();

        // Keep track of traveled distance while not hitting a wall.
        TrackDistanceWhileNotTouchingWall();
    }

    /// <summary>
    /// Applies movement input but prevents the player from leaving the circle
    /// defined by <see cref="MaxDistance"/> around <see cref="posTracker.lastSavedPosition"/>.
    /// </summary>
    private void ApplyMovementWithinBounds()
    {
        Vector2 currentPos = rb.position;
        Vector2 dirToMouse = MousePosition - currentPos;

        // Ignore very small input vectors.
        if (dirToMouse.sqrMagnitude < 0.0001f)
            return;

        Vector2 moveDir = dirToMouse.normalized;
        float deltaDistance = Speed * Time.fixedDeltaTime;

        Vector2 targetPos = currentPos + moveDir * deltaDistance;
        Vector2 center = posTracker.lastSavedPosition;

        float currentDistance = Vector2.Distance(currentPos, center);
        float targetDistance = Vector2.Distance(targetPos, center);

        // Determine if movement is trying to push the player outside the allowed radius.
        bool movingOutward = Vector2.Dot(moveDir, (currentPos - center).normalized) > 0f;
        bool atOrBeyondBound = currentDistance >= MaxDistance;

        if (atOrBeyondBound && movingOutward)
        {
            // Lock the player to the boundary and stop outward velocity.
            rb.position = center + (currentPos - center).normalized * MaxDistance;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // If the target position would exceed the boundary, clamp it.
        if (targetDistance > MaxDistance)
        {
            Vector2 clamped = center + (targetPos - center).normalized * MaxDistance;
            rb.MovePosition(clamped);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Normal movement inside the boundary.
        rb.MovePosition(targetPos);
    }

    /// <summary>
    /// Tracks total distance traveled while not touching a wall.
    /// This is separate from the max-distance boundary enforcement.
    /// </summary>
    private void TrackDistanceWhileNotTouchingWall()
    {
        Vector2 currentPosition = rb.position;
        float delta = Vector2.Distance(currentPosition, lastPosition);

        if (!wallcheck.touchingWall)
        {
            DistanceWhileNotTouchingWall += delta;
        }

        lastPosition = currentPosition;

        // If we have traveled too far, stop movement.
        if (DistanceWhileNotTouchingWall > MaxDistance)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
