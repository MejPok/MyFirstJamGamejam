using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    public List<Vector2> positions = new List<Vector2>();
    public int positionCount = 100; // Default value
    public float saveThreshold = 0.1f; // Minimum distance to save a new position
    public Vector2 lastSavedPosition;
    Wallchecker wallcheck;

    void Start()
    {
        wallcheck = transform.GetChild(0).GetComponent<Wallchecker>();

        lastSavedPosition = transform.position;
        positions.Add(lastSavedPosition); // Save initial position
    }

    void Update()
    {
        if(wallcheck.touchingWall){
            AddPosition();
        }
    }

    public void AddPosition()
    {
        if (Vector2.Distance(transform.position, lastSavedPosition) > saveThreshold)
        {
            positions.Add(transform.position);
            lastSavedPosition = transform.position;
            
            if (positions.Count > positionCount)
            {
                positions.RemoveAt(0);
            }
        }
    }
}