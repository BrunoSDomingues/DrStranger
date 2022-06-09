using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLevel : MonoBehaviour
{
    public static MainLevel Instance { get; private set; }
    
    private int lifes;
    public bool pauseTime;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        lifes = 3;       
    }

    public void setPause()
    {
        pauseTime = !pauseTime;
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
