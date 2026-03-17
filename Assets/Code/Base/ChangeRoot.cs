using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeRoot : MonoBehaviour
{
    public GameObject root;

    void Start()
    {
        root = WorldControl.instance.rootPrefab;
        UIhint.instance.AddToBases(this);
    }

    public bool PlayerInside;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInside = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInside = false;
        }
    }

    void Update()
    {
        if (PlayerInside)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartChangingRoot();
            }
        }
    }
    public Vector3 offset;
    public void StartChangingRoot()
    {
        var newRoot = Instantiate(WorldControl.instance.rootPrefab, transform.position + offset, Quaternion.identity);
        newRoot.transform.SetParent(transform);

        Destroy(this);
    }
}
