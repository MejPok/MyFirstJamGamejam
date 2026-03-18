using UnityEngine;

public class Smoke : MonoBehaviour
{
    public void AtFull()
    {
        WorldControl.instance.FullReset();
    }
}
