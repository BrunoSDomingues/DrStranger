using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject prefab;
    private GameObject bullet;

    private float offsetX = 0.4f, offsetY = 0.7f, offsetZ = 1.6f;
    private Vector3 speed;
    public static float multiplier = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    IEnumerator FireLaser()
    {
        while (true)
        {
            speed = transform.forward * multiplier;
            bullet = Instantiate(
                prefab, 
                new Vector3(
                    transform.position.x + offsetX, 
                    transform.position.y + offsetY, 
                    transform.position.z + offsetZ
                ),
                Quaternion.identity
            );
            bullet.transform.Rotate(new Vector3(90, 0, 0));
            bullet.GetComponent<Rigidbody>().AddForce(speed);
            yield return new WaitForSeconds(3);
            Destroy(bullet);
            offsetX = offsetX * (-1);
        }
    }
}
