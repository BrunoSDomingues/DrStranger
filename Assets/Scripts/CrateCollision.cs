using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateCollision : MonoBehaviour {
    private int life = 3;
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Gcollision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Sphere") hit();
    }

    private void hit() {
        life--;
        if (life <= 0) Destroy(gameObject);
    }

}
