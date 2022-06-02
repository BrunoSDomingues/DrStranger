using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLevel : MonoBehaviour
{
    private int lifes;




    void Start()
    {
        lifes = 3;       
    }

    private void hit() {
        lifes--;
        if (lifes <=0) {
            Debug.Log("Morreu");
        }
    }
    //private void OnCollisionEnter(Collision collision) {
    //    Debug.Log("with " + collision.gameObject.name);
    //    if (collision.gameObject.tag == "Sphere") hit();
    //}

    void Update()
    {
       

    }
}
