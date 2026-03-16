using UnityEngine;

public class WorldControl : MonoBehaviour
{
    public static WorldControl instance;
    public VineRibbon ribbon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public void SetNewRoot(VineRibbon rib)
    {
        ribbon = rib;
    }
}
