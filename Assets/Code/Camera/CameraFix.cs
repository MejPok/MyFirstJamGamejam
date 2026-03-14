using Unity.Cinemachine;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    public CinemachineCamera vcam;
    public CinemachineConfiner2D confiner;

    void Start()
    {
        vcam.PreviousStateIsValid = false;
        confiner.InvalidateBoundingShapeCache();
        
    }
}
