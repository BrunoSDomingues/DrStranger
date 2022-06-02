using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour {
    private GameObject player;
    public float speed = 1.0f;

    void OnEnable() {
        player = GameObject.FindWithTag("Player");
    }

    private void rotateToTarget() {
        Vector3 targetDirection = player.transform.position - transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void Update() {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 15) {
            rotateToTarget();
        }
    }
}
