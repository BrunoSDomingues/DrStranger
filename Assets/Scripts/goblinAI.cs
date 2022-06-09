using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinAI : MonoBehaviour {
    private int life;
    private GameObject player, gm;
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

    void FixedUpdate() {
        anim.enabled = !MainLevel.Instance.pauseTime;
        if (!MainLevel.Instance.pauseTime) {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= 5) {
                anim.Play("attack1");
            }
            else {
                anim.Play("idle");

            }
        }
    }
}
