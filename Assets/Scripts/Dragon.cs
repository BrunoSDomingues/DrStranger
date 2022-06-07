using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    Animator m_Animator;
    private GameObject player;
    private ParticleSystem flameParticles;
    private int dragonLife, dragonAction;
    private bool startAnimations = true, isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        flameParticles = GameObject.FindGameObjectWithTag("Flames").GetComponent<ParticleSystem>();
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
        StartCoroutine(action("takeDmg", false));

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
            if (startAnimations)
            {
                startAnimations = !startAnimations;
                StartCoroutine(startFight());
            }
        }
    }

    IEnumerator startFight()
    {
        m_Animator.SetTrigger("scream");
        m_Animator.SetTrigger("startFight");
        m_Animator.ResetTrigger("sleep");
        yield return new WaitForSeconds(7);
        m_Animator.ResetTrigger("scream");
        m_Animator.ResetTrigger("startFight");
        yield return StartCoroutine(fight());
    }

    IEnumerator fight()
    {
        Debug.Log("AAAAAAA");
        dragonAction = Random.Range(1, 3);
        switch (dragonAction)
        {
            case 1:
                StartCoroutine(action("flameAttack", true));
                break;
            case 2:
                StartCoroutine(action("defend", false));
                break;
            default:
                StartCoroutine(action("flameAttack", true));
                break;
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(fight());
    }

    IEnumerator action(string trigger, bool flames)
    {
        Debug.Log("Called action " + trigger);
        m_Animator.SetTrigger(trigger);
        if (flames) flameParticles.Play();
        yield return new WaitForSeconds(3f);
        if (flames)
        {
            flameParticles.Stop();
            flameParticles.Clear();
        }
        m_Animator.ResetTrigger(trigger);
    }
}
