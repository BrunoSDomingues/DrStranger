using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Canvases : MonoBehaviour
{
    private GameObject start, hint1, hint2, hint3, end;

    void Start()
    {
        // Main Menu
        start = GameObject.FindWithTag("StartTutorial");

        // Tutorial hints
        hint1 = GameObject.FindWithTag("Hint1");
        hint1.SetActive(false);
        hint2 = GameObject.FindWithTag("Hint2");
        hint2.SetActive(false);
        hint3 = GameObject.FindWithTag("Hint3");
        hint3.SetActive(false);
        end = GameObject.FindWithTag("EndTutorial");
        end.SetActive(false);
    }

    public void ShowControls1()
    {
        hint1.SetActive(true);
        start.SetActive(false);
    }

    // Show hints

    public void ShowControls2()
    {
        hint1.SetActive(false);
        hint2.SetActive(true);
    }

    public void ShowControls3()
    {
        hint2.SetActive(false);
        hint3.SetActive(true);
    }

    public void ShowEndTutorial()
    {
        hint3.SetActive(false);
        end.SetActive(true);
    }

    public void EndTutorial() {
        end.SetActive(false);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
