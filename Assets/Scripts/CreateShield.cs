using UnityEngine;
using Valve.VR;

public class CreateShield : MonoBehaviour
{

    public SteamVR_Action_Boolean button = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    SteamVR_Behaviour_Pose trackedObj;
    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Input_Sources hand;

    // CHECK IF FAILED
    private bool failed = false;

    // RADIUS
    public float minRadius;
    
    // BUTTONS
    private bool buttonIsPressed = false;
    
    // POSITION
    private Vector2 startPos;
    private bool storeStart = true;

    // CIRCLE
    [Range(0, 360)]private float angle;
    private bool halfCross = false;


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

        if (buttonIsPressed && !failed)
        {
            Vector2 m = moveAction[hand].axis;

            // Stores the starting position of the circle
            storeStartPos(m);

            float radius = Mathf.Sqrt(Mathf.Pow(m.x, 2.0f) + Mathf.Pow(m.y, 2));

            // If inside the minimum radius
            if (radius > minRadius)
            {
                // Calculates angle between starting and current position
                angle = Mathf.Ceil(Vector2.SignedAngle(startPos, m) * -1.0f);        
                
                // TO DO

                // Criar 4 estados
                // Estado 1: -2 < angulo < 178 e halfCross = false;
                // Estado 2: angulo >= 178 -> halfCross = true;
                // Estado 3: -2 > angulo > -178 e halfCross = true;
                // Estado 4: -2 < angulo -> completa o circulo;

                // 1 -> 2 -> 3 -> 4
                // NAO PODE QUEBRAR A ORDEM

                // OLD CODE

                //Debug.Log("ANGLE: " + angle + ", CROSSED :" + halfCross);

                //if ((!halfCross && angle < -2) || (halfCross && angle < 178))
                //{
                //    Debug.Log("ERROR: CROSS IS " + halfCross + ", ANGLE IS " + angle);
                //    failed = true;
                //}
                //else
                //{
                //    Debug.Log("OK");
                //    if (Mathf.Round(angle) >= 178) halfCross = true;
                //    if (Mathf.Round(angle) <= -177 && halfCross) Debug.Log("COMPLETE CIRCLE!");
                //}




                //Debug.Log("INVALID ANGLE!");
                //Debug.Log("Angle between start (" + startPos.x.ToString("F2") + "; " + startPos.y.ToString("F2") + ") and current (" + m.x.ToString("F2") + "; " + m.y.ToString("F2") + ") is " + angle.ToString("F2"));
            }
            else Debug.Log("OUTSIDE THE DEFINED RADIUS!");

        }
        else
        {
            if (button.GetStateUp(trackedObj.inputSource))
            {
                storeStart = true;
                failed = false;
                halfCross = false;
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
