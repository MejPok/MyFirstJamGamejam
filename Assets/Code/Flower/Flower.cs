using UnityEngine;
using UnityEngine.InputSystem;

public class Flower : MonoBehaviour
{
    public int NutrientAmount;
    public bool isPlayerInside;

    void Start()
    {
        UIhint.instance.AddToFlowers(this);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            UIhint.instance.SetHint("Press E to consume nutrients from the plant");
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isPlayerInside)
        {
            Consumed();
        }
    }

    void Consumed()
    {
        NutrientControl.instance.root.AddNutrients(NutrientAmount);
        Destroy(this.gameObject);
    }
}
