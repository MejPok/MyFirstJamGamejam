using UnityEngine;
using UnityEngine.InputSystem;

public class WorldControl : MonoBehaviour
{
    public static WorldControl instance;
    public GameObject rootPrefab;
    public VineRibbon ribbon;

    public GameObject flowerPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    public void SetNewRoot(VineRibbon rib)
    {
        if(ribbon != null && ribbon != rib)
        {
            Destroy(ribbon.gameObject);
        }
        ribbon = rib;
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ribbon.ResetWorld();
        }
    }
}
