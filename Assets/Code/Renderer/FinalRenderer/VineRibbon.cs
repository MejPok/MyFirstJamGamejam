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

    public static VineRibbon instance;
    public BasicMovement movement;

    void Start()
    {
        instance = this;
        mesh = new Mesh();
        mesh.MarkDynamic(); // Optimize for frequent updates
        GetComponent<MeshFilter>().mesh = mesh;

        // Initialize first two points for proper mesh
        Vector3 start = GetHeadPosition();
        Vector3 offset = start + Vector3.right * 0.01f; // tiny offset to make 2 points
        points.Add(start);
        points.Add(offset); 
        pointsDistance.Add(0);
        pointsDistance.Add(0);

        RebuildMesh();
    }

    void Update()
    {
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

    // Get head position in local space, optionally offset by mapRoot
    public float ppu;
    private Vector3 GetHeadPosition()
    {
        // Start with the head's world position
        Vector3 worldPos = head.position;

        // Optional: remove map root offset (e.g., if using a moving map origin)
        if (mapRoot != null)
            worldPos = mapRoot.transform.InverseTransformPoint(worldPos);

        // Convert to this GameObject's local space so the mesh is positioned correctly
        Vector3 localPos = transform.InverseTransformPoint(worldPos);

        if (pixelSnap)
        {// adjust to your pixels per unit
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
}
