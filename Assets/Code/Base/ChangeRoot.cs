using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeRoot : MonoBehaviour
{
    public GameObject root;
    public float nutrientAmount = 100f;


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
            UIhint.instance.SetHint("Press E to replant");
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
        newRoot.transform.GetChild(0).GetComponent<NutrientBase>().MaxNutrientAmount = nutrientAmount;

        Destroy(this);
    }
}
