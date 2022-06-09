using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;
using PDollarGestureRecognizer;

public class RightController : MonoBehaviour {

    // SteamVR
    public SteamVR_Action_Boolean grip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public SteamVR_Action_Boolean trigger = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    SteamVR_Behaviour_Pose trackedObj;
    public SteamVR_ActionSet actionSet;

    // Flags
    private bool detectGesture = false;
    private bool gripPressed = false;
    private bool triggerPressed = false;
    private bool isMoving = false;
    public bool creationMode = false;

    // Strings
    public string newGestureName;
    private string filePath;
    private string circleGesture = "Circle", xGesture = "X";

    // Transform
    public Transform movementSource;

    // Float values
    private float distanceThreshold = 0.02f;
    private float detectionThreshold = 0.8f;
    private float shortVib = 0.5f, longVib = 1.5f;

    // GameObjects
    public GameObject shieldPrefab;
    public GameObject attachPoint;
    public GameObject shapePrefab;
    private GameObject tempShape, shield;

    // Lists
    private List<Vector3> positionList = new List<Vector3>();
    private List<GameObject> shapes = new List<GameObject>();
    private List<Gesture> gestures = new List<Gesture>();

    private void Awake() {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Start() {
        string[] gestureFiles = Directory.GetFiles(Application.dataPath + "/XML", "*.xml");
        foreach (var item in gestureFiles) {
            gestures.Add(GestureIO.ReadGestureFromFile(item));
        }
    }


    // Update is called once per frame
    private void Update() {
        gripPressed = grip.GetStateDown(trackedObj.inputSource);
        triggerPressed = trigger.GetState(trackedObj.inputSource);

        // If grip button is pressed, switch between drawing mode/interaction mode
        if (gripPressed) {
            detectGesture = !detectGesture;
            VibrateController(shortVib);
            if (!detectGesture) StartCoroutine(Delay(1.5f));
        }

        if (detectGesture) {
            if (triggerPressed && !isMoving) {
                StartGesture();
            }
            else if (!triggerPressed && isMoving) {
                EndGesture();
            }
            else if (triggerPressed && isMoving) {
                UpdateGesture();
            }
        }
    }

    void StartGesture() {
        isMoving = true;
        Debug.Log("Started gesture");
        positionList.Clear();
        positionList.Add(movementSource.position);
        tempShape = Instantiate(shapePrefab, movementSource.position, Quaternion.identity);
        shapes.Add(tempShape);
    }

    void EndGesture() {
        isMoving = false;
        Debug.Log("Ended gesture");

        Point[] pointArray = new Point[positionList.Count];
        for (int i = 0; i < positionList.Count; i++) {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture gesture = new Gesture(pointArray);

        if (creationMode) {
            gesture.Name = newGestureName;
            gestures.Add(gesture);

            filePath = Application.dataPath + "/XML/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, filePath);
        }
        else {
            // Checks if a previous shield exists, and if yes, destroys it
            shield = GameObject.FindWithTag("Shield");
            if (shield) Destroy(shield);

            if (positionList.Count > 1) {
                Result result = PointCloudRecognizer.Classify(gesture, gestures.ToArray());

                // Checks if result score is above threshold
                if (result.Score > detectionThreshold) {
                    string detectedGesture = result.GestureClass;

                    if (detectedGesture == circleGesture) {
                        // Instantiates new shield
                        shield = GameObject.Instantiate(shieldPrefab);

                        // Sets shield as child of hand
                        shield.transform.parent = attachPoint.transform;

                        // Sets localPosition and localRotation
                        shield.transform.localPosition = new Vector3(-0.05f, -0.03f, -0.09f);
                        shield.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 45));
                    }
                    else if (detectedGesture == xGesture) {
                        Debug.Log("X!");
                        StartCoroutine(StopTime());
                    }
                    else VibrateController(longVib);
                }
                else VibrateController(longVib);
            }
            else VibrateController(longVib);
            StartCoroutine(DestroyDrawing(1.5f));
        }
    }

    void UpdateGesture() {
        Debug.Log("Updating gesture");
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > distanceThreshold) {
            positionList.Add(movementSource.position);
            tempShape = Instantiate(shapePrefab, movementSource.position, Quaternion.identity);
            shapes.Add(tempShape);
        }
    }

    void VibrateController(float duration) {
        SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.RightHand].Execute(0, duration, 10, 1);
    }

    IEnumerator StopTime() {
        Debug.Log("parei tempo!");
        MainLevel.Instance.setPause();
        yield return new WaitForSecondsRealtime(5f);
        Debug.Log("Voltei tempo!");
        MainLevel.Instance.setPause();
    }

    IEnumerator Delay(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);
        VibrateController(shortVib);
    }

    IEnumerator DestroyDrawing(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);
        foreach (GameObject shape in shapes) Destroy(shape);
    }
}
