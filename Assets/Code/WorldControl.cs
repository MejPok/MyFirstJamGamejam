using UnityEngine;

public class WorldControl : MonoBehaviour
{
    public static WorldControl instance;
    public GameObject rootPrefab;
    public VineRibbon ribbon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    public void SetNewRoot(VineRibbon rib)
    {
        if(ribbon != null)
        {
            Destroy(ribbon.gameObject);
        }
        ribbon = rib;
    }
}
