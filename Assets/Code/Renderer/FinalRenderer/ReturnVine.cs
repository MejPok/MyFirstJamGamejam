using UnityEngine;
using UnityEngine.InputSystem;

public class ReturnVine : MonoBehaviour
{
    static public ReturnVine instance;
    public bool returningVine;
    void Start()
    {
        instance = this;
    }
    float timer;
    public float timeBetweenMove;
    void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            returningVine = true;
            if(timer > timeBetweenMove)
            {
                
                int index = WorldControl.instance.ribbon.points.Count - 1;
                if (index > 2)
                {
                    // Points are stored in the vine's local space, with the vine object
                    // positioned at mapRoot (if assigned). So just transform from vine-local to world.
                    Vector3 localPoint = WorldControl.instance.ribbon.points[index];
                    Vector3 worldPos = WorldControl.instance.ribbon.transform.TransformPoint(localPoint);

                    transform.position = worldPos;
                    WorldControl.instance.ribbon.points.RemoveAt(index);

                    GetComponent<BasicMovement>().DistanceWhileNotTouchingWall = WorldControl.instance.ribbon.pointsDistance[index];
                    Debug.Log("Set new distance to " + WorldControl.instance.ribbon.pointsDistance[index]);
                    WorldControl.instance.ribbon.pointsDistance.RemoveAt(index);
                    transform.rotation = WorldControl.instance.ribbon.pointsRotation[index];
                    WorldControl.instance.ribbon.pointsRotation.RemoveAt(index);

                }
                
                timer = 0;
            } else
            {
                timer += Time.deltaTime;
            }
           
            //GetComponent<Rigidbody2D>().position
            
        } else
        {
            returningVine = false;
        }
    }
}
