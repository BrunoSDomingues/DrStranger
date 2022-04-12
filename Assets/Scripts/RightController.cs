using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;
using PDollarGestureRecognizer;

public class RightController : MonoBehaviour
{
    // SteamVR
    public SteamVR_Action_Boolean grip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public SteamVR_Action_Boolean trigger = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    SteamVR_Behaviour_Pose trackedObj;
    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Input_Sources hand;

    // Shield flags
    private bool shieldOK = false;
    private bool shieldFail = false;
    private bool gripPressed = false;
    private bool storeStart = true;

    // Gesture flags
    private bool triggerPressed = false;
    private bool isMoving = false;
    public bool creationMode = true;

    // Strings
    public string newGestureName;
    private string filePath;
    private string expectedGesture = "Circle";

    // Vectors
    private Vector2 startPos;

    // Transform
    public Transform movementSource;

    // Float values
    private float startAngle = 0f, curAngle, maxAngleDiff = 7f;
    private float minRadius;
    private float distanceThreshold = 0.02f;
    private float detectionThreshold = 0.9f;

    // GameObjects
    public GameObject shieldPrefab;
    public GameObject attachPoint;
    public GameObject shapePrefab;
    private GameObject tempShape;

    // Lists
    private List<Vector3> positionList = new List<Vector3>();
    private List<GameObject> shapes = new List<GameObject>();
    private List<Gesture> gestures = new List<Gesture>();

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Start()
    {
        minRadius = 0.5f;
        actionSet.Activate(hand);
        string[] gestureFiles = Directory.GetFiles(Application.dataPath, "*.xml");
        foreach (var item in gestureFiles)
        {
            gestures.Add(GestureIO.ReadGestureFromFile(item));
        }
    }


    // Update is called once per frame
    private void Update()
    {
        gripPressed = grip.GetState(trackedObj.inputSource);
        triggerPressed = trigger.GetState(trackedObj.inputSource);

        switch (LeftController.detectGesture)
        {
            case (true):
                if (triggerPressed && !isMoving)
                {
                    StartGesture();
                }
                else if (!triggerPressed && isMoving)
                {
                    EndGesture();
                }
                else if (triggerPressed && isMoving)
                {
                    UpdateGesture();
                }
                    
                break;

            case (false):
                if (gripPressed && !shieldOK && !shieldFail)
                {
                    // Destroys previous shield when pressing button again
                    DestroyShield();

                    // Stores hand
                    Vector2 m = moveAction[hand].axis;

                    // Stores the starting position of the circle
                    storeStartPos(m);

                    // Checks if the shield was drawn correctly
                    CheckShield(m);
                }

                else if (!gripPressed)
                {
                    // Handler for when player releases button
                    GripRelease(grip);
                }

                if (shieldFail)
                {
                    Debug.Log("VIBRATE!");
                    SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.RightHand].Execute(0, 1.5f, 10, 1);
                }

                break;
        }
    }

    void DestroyShield()
    {
        // Checks if the previous shield has failed or if the player was not shieldOKful
        if (!shieldOK || shieldFail)
        {
            // Looks for shield, and if it exists, destroys it
            GameObject shield = GameObject.FindWithTag("Shield");
            if (shield != null)
            {
                Destroy(shield);
                Debug.Log("Destruindo escudo previo...");
            }
        }
    }

    void CheckShield(Vector2 pos)
    {
        float radius = Mathf.Sqrt(Mathf.Pow(pos.x, 2.0f) + Mathf.Pow(pos.y, 2.0f));

        // If inside the minimum radius
        if (radius > minRadius)
        {
            // Calculates angle between starting and current position
            curAngle = Mathf.Round(Vector2.SignedAngle(startPos, pos) * (-100f)) / 100f;
            curAngle = (curAngle >= 0) ? curAngle : curAngle + 360f;

            // Checks if angle difference is too big
            if (Mathf.Abs(curAngle - startAngle) >= maxAngleDiff)
            {
                Debug.Log("Erro: diferenca de angulos = " + Mathf.Abs(curAngle - startAngle));
                shieldFail = true;
            }

            else
            {
                Debug.Log("Direcao certa!");

                // Full circle
                if (curAngle >= 358)
                {
                    Debug.Log("Escudo completo!");
                    shieldOK = true;
                }

                startAngle = curAngle;
            }
        }
        else
        {
            Debug.Log("Erro: raio do circulo muito pequeno!");
            shieldFail = true;
        }
    }

    void GripRelease(SteamVR_Action_Boolean but)
    {
        if (but.GetStateUp(trackedObj.inputSource))
        {
            if (shieldOK && !shieldFail)
            {
                Debug.Log("Criando escudo...");

                // Instantiates shield
                GameObject shield = GameObject.Instantiate(shieldPrefab);

                // Sets shield as child of hand
                shield.transform.parent = attachPoint.transform;

                // Sets localPosition and localRotation
                shield.transform.localPosition = new Vector3(-0.05f, -0.03f, -0.09f);
                shield.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 45));

                Debug.Log("Escudo criado!");
            }

            // Resets parameters
            Debug.Log("Resetando parametros...");
            storeStart = true;
            shieldOK = false;
            shieldFail = false;
            startAngle = 0f;
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

    void StartGesture()
    {
        isMoving = true;
        Debug.Log("1");
        positionList.Clear();
        positionList.Add(movementSource.position);
        tempShape = Instantiate(shapePrefab, movementSource.position, Quaternion.identity);
        shapes.Add(tempShape);
    }

    void EndGesture()
    {
        isMoving = false;
        Debug.Log("2");

        Point[] pointArray = new Point[positionList.Count];
        for (int i = 0; i < positionList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture gesture = new Gesture(pointArray);

        if (creationMode)
        {
            gesture.Name = newGestureName;
            gestures.Add(gesture);

            filePath = Application.dataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, filePath);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(gesture, gestures.ToArray());
            if (result.GestureClass == expectedGesture && result.Score > detectionThreshold)
            {
                Debug.Log("CIRCLE!");
                StartCoroutine(StopTime());
            }
            foreach (GameObject shape in shapes) Destroy(shape);
        }
    }

    void UpdateGesture()
    {
        Debug.Log("3");
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > distanceThreshold)
        {
            positionList.Add(movementSource.position);
            tempShape = Instantiate(shapePrefab, movementSource.position, Quaternion.identity);
            shapes.Add(tempShape);
        }
    }

    IEnumerator StopTime()
    {
        Laser.multiplier = 0;
        yield return new WaitForSeconds(5);
        Laser.multiplier = 1000;
    }
}
