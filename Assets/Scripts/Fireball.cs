using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fireball : MonoBehaviour
{
    private static float globalGravity = -6f;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * Vector3.up;
        rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
}
