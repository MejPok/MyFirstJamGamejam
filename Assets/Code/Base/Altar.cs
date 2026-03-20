using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Altar : MonoBehaviour
{
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
        if (Keyboard.current.eKey.wasPressedThisFrame && PlayerInside)
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }




}
