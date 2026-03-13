using UnityEngine;

public class Wallchecker : MonoBehaviour
{
    public bool touchingWall;

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 6)
        {
            touchingWall = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 6)
        {
            touchingWall = false;
        }
    }
}
