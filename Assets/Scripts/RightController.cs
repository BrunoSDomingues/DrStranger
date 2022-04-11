using UnityEngine;
using Valve.VR;

public class RightController : MonoBehaviour
{
    // SteamVR
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
    private float startAngle = 0f, curAngle, maxAngleDiff = 7f;
    private float minRadius;

    // PREFAB
    public GameObject prefab;
    public GameObject attachPoint;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Start()
    {
        minRadius = 0.5f;
        actionSet.Activate(hand);
    }


    // Update is called once per frame
    private void Update()
    {
        buttonIsPressed = button.GetState(trackedObj.inputSource);   

        if (buttonIsPressed && !success && !failed)
        {
            if (!success || failed)
            {
                GameObject shield = GameObject.FindWithTag("Shield");
                if (shield != null)
                {
                    Destroy(shield);
                    Debug.Log("Destruindo escudo previo...");
                }
            }

            Vector2 m = moveAction[hand].axis;

            // Stores the starting position of the circle
            storeStartPos(m);

            float radius = Mathf.Sqrt(Mathf.Pow(m.x, 2.0f) + Mathf.Pow(m.y, 2));

            // If inside the minimum radius
            if (radius > minRadius)
            {
                // Calculates angle between starting and current position

                curAngle = Mathf.Round(Vector2.SignedAngle(startPos, m) * (-100f))/100f;
                curAngle = (curAngle >= 0) ? curAngle : curAngle + 360f;
                if (Mathf.Abs(curAngle - startAngle) >= maxAngleDiff)
                {
                    Debug.Log("Erro: diferenca de angulos = " + Mathf.Abs(curAngle - startAngle));
                    failed = true;
                }

                else
                {
                    Debug.Log("Direcao certa!");

                    if (curAngle >= 358)
                    {
                        Debug.Log("Escudo completo!");
                        success = true;
                    }

                    startAngle = curAngle;
                }
            }
            else
            {
                Debug.Log("Erro: raio do circulo muito pequeno!");
                failed = true;
            }

        }
        else
        {
            if (button.GetStateUp(trackedObj.inputSource))
            {
                if (success && !failed)
                {
                    Debug.Log("Criando escudo...");
                    GameObject shield = GameObject.Instantiate(prefab);
                    shield.transform.parent = attachPoint.transform;
                    shield.transform.localPosition = new Vector3(-0.05f, -0.03f, -0.09f);
                    shield.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 45));

                    //joint = shield.AddComponent<FixedJoint>();
                    //joint.connectedBody = attachPoint;
                    Debug.Log("Escudo criado!");
                }

                Debug.Log("Resetando parametros...");
                storeStart = true;
                success = false;
                failed = false;
                startAngle = 0f;
            }
        }

        if (failed)
        {
            Debug.Log("VIBRATE!");
            SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.RightHand].Execute(0, 1.5f, 10, 1);
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
