using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject prefab, bullet;

    private Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = transform.forward * 1000;
        StartCoroutine(FireLaser());
        
    }

    // Update is called once per frame
    IEnumerator FireLaser()
    {
        while (true)
        {
            bullet = Instantiate(prefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(speed);
            yield return new WaitForSeconds(3);
            Destroy(bullet);
        }
    }
}
