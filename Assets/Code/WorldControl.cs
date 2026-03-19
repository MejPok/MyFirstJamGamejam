using UnityEngine;
using UnityEngine.InputSystem;

public class WorldControl : MonoBehaviour
{
    public static WorldControl instance;
    public GameObject rootPrefab;
    public VineRibbon ribbon;

    public GameObject flowerPrefab;
    public GameObject BlackScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    public void SetNewRoot(VineRibbon rib)
    {
        if(ribbon != null && ribbon != rib)
        {
            ribbon.Freezed = true;
        }
        ribbon = rib;
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            
            BlackScreen.GetComponent<Animator>().SetTrigger("Smoke");
        }
    }

    public void FullReset()
    {
        ribbon.ResetWorld();
    }
}
