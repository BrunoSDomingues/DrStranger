using UnityEngine;
using Valve.VR;

public class CreateShield : MonoBehaviour
{

    public SteamVR_Action_Boolean button = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    SteamVR_Behaviour_Pose trackedObj;
    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Input_Sources hand;

    // FLAGS
    private bool success = false;
    private bool failed = false;
    private bool buttonIsPressed = false;
    private bool storeStart = true;

    // VALUES
    private Vector2 startPos;
    private float startAngle = 0f, curAngle;
    public float minRadius;


    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Start()
    {
        minRadius = 0.6f;
        actionSet.Activate(hand);
    }


    // Update is called once per frame
    private void Update()
    {
        buttonIsPressed = button.GetState(trackedObj.inputSource);   

        if (buttonIsPressed && !success)
        {
            Vector2 m = moveAction[hand].axis;

            // Stores the starting position of the circle
            storeStartPos(m);

            float radius = Mathf.Sqrt(Mathf.Pow(m.x, 2.0f) + Mathf.Pow(m.y, 2));

            // If inside the minimum radius
            if (radius > minRadius)
            {
                // Calculates angle between starting and current position
                curAngle = Mathf.Round(Vector2.SignedAngle(startPos, m) * (-100f))/100f;
                curAngle = (curAngle > 0) ? curAngle : curAngle + 360f;

                if (Mathf.Abs(curAngle - startAngle) >= 4)
                {
                    Debug.Log("SAIU");
                    failed = true;
                }

                else
                {
                    Debug.Log("OK");

                    if (curAngle >= 358)
                    {
                        Debug.Log("COMPLETE CIRCLE");
                        success = true;
                    }

                    startAngle = curAngle;
                }
            }
            else
            {
                Debug.Log("OUTSIDE THE DEFINED RADIUS!");
                failed = true;
            }

        }
        else
        {
            if (button.GetStateUp(trackedObj.inputSource))
            {
                storeStart = true;
                success = false;
                failed = false;
                startAngle = 0f;
            }
        }
            
        
    }

    private void storeStartPos(Vector2 pos)
    {
        if (storeStart)
        {
            startPos = pos;
            storeStart = false;
        }
    }
}
