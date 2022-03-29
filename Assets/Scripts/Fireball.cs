using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private static float globalGravity = -6f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
