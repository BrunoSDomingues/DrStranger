using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Sphere") Destroy(gameObject);
    }
}
