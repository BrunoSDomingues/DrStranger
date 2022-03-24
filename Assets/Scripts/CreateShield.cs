using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class CreateShield : MonoBehaviour
{

    public SteamVR_Action_Boolean button = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    SteamVR_Behaviour_Pose trackedObj;
    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Input_Sources hand;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Start() {
        actionSet.Activate(hand);
    }


    // Update is called once per frame
    private void Update()
    {
        if (button.GetStateDown(trackedObj.inputSource))
        {
            Vector2 m = moveAction[hand].axis;

            Debug.Log("x: " + m.x + ", y: " + m.y);
        };
    }
}
