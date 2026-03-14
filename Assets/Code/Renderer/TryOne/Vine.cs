using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Vine : MonoBehaviour
{
    public Transform head;                  // The moving vine head
    public float segmentSpacing = 0.1f;     // How far between points

    private LineRenderer line;
    private List<Vector3> positions = new List<Vector3>();
    private float distanceSinceLastPoint = 0f;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        positions.Add(head.position);
        line.positionCount = 1;
        line.SetPosition(0, head.position);
    }

    void Update()
    {
        // Measure distance from last point
        float distance = Vector3.Distance(head.position, positions[positions.Count - 1]);
        distanceSinceLastPoint += distance;

        if (distanceSinceLastPoint >= segmentSpacing)
        {
            AddPoint(head.position);
            distanceSinceLastPoint = 0f;
        }
    }

    void AddPoint(Vector3 newPos)
    {
        positions.Add(newPos);
        line.positionCount = positions.Count;
        line.SetPosition(positions.Count - 1, newPos);
    }
}
