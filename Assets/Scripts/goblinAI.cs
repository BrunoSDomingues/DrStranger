using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinAI : MonoBehaviour {
    private int life;
    private GameObject player;

    private Animation anim;

    void Start() {
        life = 3;
        anim = gameObject.GetComponent<Animation>();

        player = GameObject.FindWithTag("Player");

    }
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Gcollision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Sphere") hit();
    }

    private void hit() {
        life--;
        if (life <= 0) Destroy(gameObject);
    }


    // Update is called once per frame
    void FixedUpdate() {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 3) {
            anim.Play("attack1");
        }
        else {
            anim.Play("idle");

        }
    }
}
