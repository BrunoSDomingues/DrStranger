using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject prefab;
    private GameObject bullet;

    private float offsetX = 0.4f, offsetY = 0.7f, offsetZ = 1.6f;
    private Vector3 prevSpeed, impulse;
    private Rigidbody rb;
    public static bool timeStop = false;

    // Start is called before the first frame update
    void Start()
    {
        impulse = transform.forward * 1500f;
        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    IEnumerator FireLaser()
    {
        while (true)
        {
            if (!timeStop)
            {
                bullet = Instantiate(
                    prefab,
                    new Vector3(
                        transform.position.x + offsetX,
                        transform.position.y + offsetY,
                        transform.position.z + offsetZ
                    ),
                    Quaternion.identity
                );
            }
            if (bullet)
            {
                rb = bullet.GetComponent<Rigidbody>();
                bullet.transform.Rotate(new Vector3(90, 0, 0));

                if (!timeStop) rb.AddForce(impulse);
                else rb.velocity = Vector3.zero;
                Destroy(bullet, 3f);
                offsetX = offsetX * (-1);
            }
            yield return new WaitForSeconds(3);
        }
    }
}
