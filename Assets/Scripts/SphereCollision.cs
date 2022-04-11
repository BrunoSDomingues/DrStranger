using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Crate") Destroy(gameObject);
    }
}
