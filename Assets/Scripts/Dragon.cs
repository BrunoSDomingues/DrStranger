using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{

    Animator m_Animator;
    private GameObject player;
    private int dragonLife, dragonAction;
    private bool startAnimations = true, isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        dragonLife = 3;
        m_Animator.SetTrigger("sleep");

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Dragon collision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Sphere" && !isDead) hit();
    }

    private void hit()
    {   
        dragonLife--;
        StartCoroutine(action("takeDmg"));

        if (dragonLife <= 0)
        {
            m_Animator.SetTrigger("die");
            isDead = true;
            //Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < 50)
        {
            if (startAnimations) {
                StartCoroutine(startFight());
                startAnimations = !startAnimations;
            }
            StartCoroutine(fight());
        }
    }

    IEnumerator startFight()
    {
        m_Animator.SetTrigger("scream");
        m_Animator.SetTrigger("startFight");
        m_Animator.ResetTrigger("sleep");
        yield return new WaitForSeconds(4);
        m_Animator.ResetTrigger("scream");
        m_Animator.ResetTrigger("startFight");
        yield break;
    }

    IEnumerator fight()
    {
        dragonAction = Random.Range(1, 3);
        switch (dragonAction)
        {
            case 1:
                StartCoroutine(action("flameAttack"));
                break;
            case 2:
                StartCoroutine(action("defend"));
                break;
            default:
                StartCoroutine(action("flameAttack"));
                break;
        }
        yield return new WaitForSeconds(7);
    }

    IEnumerator action(string trigger)
    {
        m_Animator.SetTrigger(trigger);
        yield return new WaitForSeconds(5);
        m_Animator.ResetTrigger(trigger);
    }
}
