using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    private static float globalGravity = -6f;
    private Rigidbody rb;
    private GameObject goblin;

    public Material ice, fire;
    private bool ballType = false;

    // Start is called before the first frame update
    void OnEnable() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        GetComponent<Renderer>().material = ice;
    }

    void setMaterial() {
        // set material
        if (ballType) {
            GetComponent<Renderer>().material = ice;
        }
        else {
            GetComponent<Renderer>().material = fire;
        }
        ballType = !ballType;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 gravity = globalGravity * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

}
