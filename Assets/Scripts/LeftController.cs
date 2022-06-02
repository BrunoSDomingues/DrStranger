using UnityEngine;
using Valve.VR.Extras;
using Valve.VR;

public class LeftController : MonoBehaviour {
    public GameObject prefab;
    public Rigidbody attachPoint;

    public SteamVR_Action_Boolean trigger = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    public SteamVR_Action_Boolean grip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    SteamVR_Behaviour_Pose trackedObj;
    FixedJoint joint = null;

    private void Awake() {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void FixedUpdate() {
        if (joint == null && trigger.GetStateDown(trackedObj.inputSource)) {
            GameObject go = Instantiate(prefab);
            go.transform.position = attachPoint.transform.position;

            joint = go.AddComponent<FixedJoint>();
            joint.connectedBody = attachPoint;
        }
        else if (joint != null && trigger.GetStateUp(trackedObj.inputSource)) {
            GameObject go = joint.gameObject;
            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;
            Object.Destroy(go, 15.0f);
            Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
            if (origin != null) {
                rigidbody.velocity = origin.TransformVector(trackedObj.GetVelocity());
                rigidbody.angularVelocity = origin.TransformVector(trackedObj.GetAngularVelocity());
            }
            else {
                rigidbody.velocity = trackedObj.GetVelocity();
                rigidbody.angularVelocity = trackedObj.GetAngularVelocity();
            }
            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }
    }
}
