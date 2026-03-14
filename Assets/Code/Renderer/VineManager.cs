using UnityEngine;

public class VineManager : MonoBehaviour
{
    public Transform head;
    public GameObject vineTilePrefab;

    public float tileSpacing = 0.3f;
    public float turnSmooth = 6f;

    private Vector3 lastSpawnPos;
    private Vector3 lastDirection;

    void Start()
    {
        lastSpawnPos = head.position;
        lastDirection = head.right;

        SpawnTile(lastSpawnPos, lastDirection);
    }

    void Update()
    {
        Vector3 headDir = (head.position - lastSpawnPos).normalized;

        // Smooth direction change
        lastDirection = Vector3.Lerp(lastDirection, headDir, turnSmooth * Time.deltaTime).normalized;

        float distance = Vector3.Distance(head.position, lastSpawnPos);

        if (distance >= tileSpacing)
        {
            lastSpawnPos += lastDirection * tileSpacing;

            SpawnTile(lastSpawnPos, lastDirection);
        }
    }

    void SpawnTile(Vector3 position, Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Instantiate(
            vineTilePrefab,
            position,
            Quaternion.Euler(0, 0, angle),
            transform
        );
    }
}