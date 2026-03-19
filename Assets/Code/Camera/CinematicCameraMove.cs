using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicCameraMove : MonoBehaviour
{
    bool move;
    public Transform depth;
    public void CinematicMove(){
        move = true;
    }

    void Update()
    {
        if(transform.position.y >= depth.position.y && move)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, -10);
        }
        if(transform.position.y <= depth.position.y)
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
