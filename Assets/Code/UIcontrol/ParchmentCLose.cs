using UnityEngine;
using UnityEngine.InputSystem;

public class ParchmentClose : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            this.gameObject.SetActive(false);
        }
    }


}
