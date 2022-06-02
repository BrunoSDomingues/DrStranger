using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    public GameObject prefab;
    public static bool timeStop = false;

    private GameObject bullet, player;

    private Vector3 impulse;
    private Rigidbody rb;


    void OnEnable() {
        player = GameObject.FindWithTag("Player");
        impulse = transform.forward * 1500f;
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser() {
        while (true) {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= 25) {
                if (!timeStop) {
                    bullet = Instantiate(
                        prefab,
                        transform.position,
                        Quaternion.identity
                    );
                }
                if (bullet) {
                    rb = bullet.GetComponent<Rigidbody>();
                    bullet.transform.Rotate(new Vector3(90, 0, 0));

                    if (!timeStop) rb.AddForce(impulse);

                    else rb.velocity = Vector3.zero;
                    Destroy(bullet, 3f);
                }
            }
            yield return new WaitForSeconds(3);
        }
    }
}
