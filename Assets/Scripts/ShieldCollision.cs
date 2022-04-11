using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    public static float blockedShots = 0;
    private GameObject laser;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Laser")
        {
            blockedShots++;
            laser = GameObject.FindWithTag("Laser");
            if (laser != null) Destroy(laser);
        }
    }
}
