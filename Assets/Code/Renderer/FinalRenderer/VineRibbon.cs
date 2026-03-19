using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VineRibbon : MonoBehaviour
{
    [Header("Vine Settings")]
    public Transform head;             // The head that moves
    public GameObject mapRoot;         // Optional: for world offsets
    public float pointSpacing = 0.05f; // Distance between points
    public float width = 1f;           // Vine width in Unity units
    public float textureRepeatLength = 1f; // How much length one texture repeat covers
    public bool pixelSnap = true;      // Snap to pixels

    public List<Vector3> points = new List<Vector3>();
    public List<float> pointsDistance = new List<float>();
    public List<Quaternion> pointsRotation = new List<Quaternion>();


    [Header("Sway")]
    public float swayAmplitude = 0.05f;
    public float swaySpeed = 3f;
    public float swayFrequency = 4f;

    private Mesh mesh;
    private Vector3[] verts;
    private Vector2[] uvs;
    private int[] tris;
    private float[] cumulativeLength;
    private bool meshNeedsRebuild = false;

    // Keep track of where the head started so we can correctly restore it on reset.
    private Vector3 initialHeadPosition;
    private Quaternion initialHeadRotation;
    private float initialDistanceWhileNotTouchingWall;

    public BasicMovement movement;

    public bool Freezed;


    void Start()
    {
        WorldControl.instance.SetNewRoot(this);
        var Player = GameObject.FindWithTag("Player");
        head = Player.transform;
        movement = Player.GetComponent<BasicMovement>();

        // Cache the head's starting transform so we can restore it when resetting the world.
        initialHeadPosition = head.position;
        initialHeadRotation = head.rotation;
        initialDistanceWhileNotTouchingWall = movement.DistanceWhileNotTouchingWall;

        mesh = new Mesh();
        mesh.MarkDynamic(); // Optimize for frequent updates
        GetComponent<MeshFilter>().mesh = mesh;

        // If a map root is provided, keep this object positioned at it so the mesh can start from (0,0) in local space.
        if (mapRoot != null)
            transform.position = mapRoot.transform.position;

        // Start the mesh from the local origin (map root) and grow toward the head position
        points.Add(Vector3.zero);
        points.Add(GetHeadPosition());

        pointsDistance.Add(0);
        pointsDistance.Add(0);

        pointsRotation.Add(movement.transform.rotation);
        pointsRotation.Add(movement.transform.rotation);


        RebuildMesh();
        ResetAllFlowers();
    }
    bool setMySelf;
    void Update()
    {
        if (Freezed)
        {
            return;
        }

        // Keep this object pinned to the map root if assigned, so local (0,0) stays at the map root.
        if (mapRoot != null)
            transform.position = mapRoot.transform.position;

        // Keep the first point anchored at local zero
        if (points.Count > 0)
            points[0] = Vector3.zero;

        if (ReturnVine.instance.returningVine)
        {
            meshNeedsRebuild = true;
        }
        else
        {
            Vector3 headPos = GetHeadPosition();
            float dist = Vector3.Distance(points[points.Count - 1], headPos);
            if (dist >= pointSpacing)
            {
                points.Add(headPos);
                pointsDistance.Add(movement.DistanceWhileNotTouchingWall);
                pointsRotation.Add(movement.transform.rotation);

                meshNeedsRebuild = true;
            }
        }

        if (meshNeedsRebuild)
        {
            RebuildMesh();
            meshNeedsRebuild = false;
        }

        UpdateSway();
    }

    // Get head position in this object's local space.
    // If a mapRoot is set, this object is kept at mapRoot's world position so the vine starts at (0,0) in local space.
    public float ppu;
    private Vector3 GetHeadPosition()
    {
        // Convert the head's world position to this GameObject's local space
        Vector3 localPos = transform.InverseTransformPoint(head.position);

        if (pixelSnap)
        { // adjust to your pixels per unit
            localPos.x = Mathf.Round(localPos.x * ppu) / ppu;
            localPos.y = Mathf.Round(localPos.y * ppu) / ppu;
        }
        return localPos;
    }

    public float TotalDistance;
    private void RebuildMesh()
    {
        if (points.Count < 2)
            return; // cannot build mesh with less than 2 points

        int vertCount = points.Count * 2;
        verts = new Vector3[vertCount];
        uvs = new Vector2[vertCount];
        tris = new int[(points.Count - 1) * 6];
        cumulativeLength = new float[points.Count];

        // Compute cumulative distances along the vine
        float totalLength = 0f;
        cumulativeLength[0] = 0f;
        for (int i = 1; i < points.Count; i++)
        {
            float d = Vector3.Distance(points[i], points[i - 1]);
            totalLength += d;
            cumulativeLength[i] = totalLength;
        }
        TotalDistance = totalLength;

        // Build vertices and UVs (base positions without sway)
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 forward;
            if (i == 0)
                forward = (points[i + 1] - points[i]).normalized;
            else if (i == points.Count - 1)
                forward = (points[i] - points[i - 1]).normalized;
            else
                forward = ((points[i + 1] - points[i]).normalized + (points[i] - points[i - 1]).normalized).normalized;

            Vector3 normal = new Vector3(-forward.y, forward.x, 0f);
            Vector3 meshPos = points[i];
            meshPos.z = 0f; // ensure visible in 2D

            verts[i * 2] = meshPos + normal * width * 0.5f;
            verts[i * 2 + 1] = meshPos - normal * width * 0.5f;

            // Use cumulative distance for texture UV
            float v = cumulativeLength[i] / textureRepeatLength;
            uvs[i * 2] = new Vector2(0, v);
            uvs[i * 2 + 1] = new Vector2(1, v);
        }

        // Build triangles
        int triIndex = 0;
        for (int i = 0; i < points.Count - 1; i++)
        {
            int vi = i * 2;

            tris[triIndex++] = vi;
            tris[triIndex++] = vi + 2;
            tris[triIndex++] = vi + 1;

            tris[triIndex++] = vi + 1;
            tris[triIndex++] = vi + 2;
            tris[triIndex++] = vi + 3;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
    }

    private void UpdateSway()
    {
        if (verts == null || points.Count < 2)
            return;

        // Update vertices for sway (only modify positions where pointsDistance > 0)
        for (int i = 0; i < points.Count; i++)
        {
            if (pointsDistance[i] > 0) // only sway when detached
            {
                Vector3 forward;
                if (i == 0)
                    forward = (points[i + 1] - points[i]).normalized;
                else if (i == points.Count - 1)
                    forward = (points[i] - points[i - 1]).normalized;
                else
                    forward = ((points[i + 1] - points[i]).normalized + (points[i] - points[i - 1]).normalized).normalized;

                Vector3 normal = new Vector3(-forward.y, forward.x, 0f);
                Vector3 meshPos = points[i];
                float wave = Mathf.Sin(Time.time * swaySpeed + cumulativeLength[i] * swayFrequency);
                meshPos += normal * wave * swayAmplitude;
                meshPos.z = 0f;

                verts[i * 2] = meshPos + normal * width * 0.5f;
                verts[i * 2 + 1] = meshPos - normal * width * 0.5f;
            }
        }

        mesh.vertices = verts; // Update the mesh with new vertex positions
    }

    public void ResetAllFlowers()
    {
        if(transform.parent.parent != null)
        {
            var flowerHolder = transform.parent.parent.GetComponent<FlowerHolder>();
            flowerHolder.ResetFlowers();
        }
        
    }

    public void ResetWorld()
    {
        // Restore the player head to the same position/rotation it started at.
        movement.ResetWorld(initialHeadPosition);

        head.transform.position = initialHeadPosition;
        head.transform.rotation = initialHeadRotation;
        movement.DistanceWhileNotTouchingWall = initialDistanceWhileNotTouchingWall;

        points.Clear();
        pointsDistance.Clear();
        pointsRotation.Clear();
        cumulativeLength = null;
        ResetAllFlowers();
        Start();
    }

    
}
