using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    Animator m_Animator;

    private GameObject player, arrowSpawn, arrow;
    public GameObject arrowPrefab;
    private Rigidbody rb;

    public float speed = 1.0f;
    private int archerLife;
    private bool die = false;


    void OnEnable()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("MainCamera");
        arrowSpawn = GameObject.FindWithTag("LaserSpawn");
        archerLife = 3;
    }

    private void rotateToTarget()
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Archer collision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Sphere") hit();
    }

    private void hit()
    {
        archerLife--;
        if (archerLife <= 0)
        {
            m_Animator.SetTrigger("die");
            die = true;
            //Destroy(gameObject);
        }
    }


    void Update()
    {
        m_Animator.speed = MainLevel.Instance.pauseTime ? 0 : 1;
        if (!die || !MainLevel.Instance.pauseTime)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= 15)
            {
                rotateToTarget();
                m_Animator.SetTrigger("shoot");
            }
        }
    }

    public void shootArrow()
    {
        arrow = Instantiate(
                        arrowPrefab,
                        arrowSpawn.transform.position,
                        Quaternion.identity
                    );

        if (arrow)
        {
            arrow.transform.LookAt(player.transform);
            //arrow.transform.Rotate(new Vector3(-90, 0, 0));

            rb = arrow.GetComponent<Rigidbody>();

            rb.AddForce(arrowSpawn.transform.forward * 5f, ForceMode.Impulse);
            //rb.velocity = transform.TransformDirection(new Vector3(2, 2, 5));

            Destroy(arrow, 3f);
        }
    }
}
