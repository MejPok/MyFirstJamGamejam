using UnityEditor.Callbacks;
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
                
                int index = VineRibbon.instance.points.Count - 1;
                if (index > 2)
                {
                    // Points are stored in the vine's local space (after mapRoot offset), so
                    // convert back to world space before moving the return object.
                    Vector3 localPoint = VineRibbon.instance.points[index];
                    Vector3 worldPos = VineRibbon.instance.transform.TransformPoint(localPoint);
                    if (VineRibbon.instance.mapRoot != null)
                        worldPos = VineRibbon.instance.mapRoot.transform.TransformPoint(worldPos);

                    transform.position = worldPos;
                    VineRibbon.instance.points.RemoveAt(index);

                    GetComponent<BasicMovement>().DistanceWhileNotTouchingWall = VineRibbon.instance.pointsDistance[index];
                    Debug.Log("Set new distance to " + VineRibbon.instance.pointsDistance[index]);
                    VineRibbon.instance.pointsDistance.RemoveAt(index);

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
