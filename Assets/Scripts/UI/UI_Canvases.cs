using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Canvases : MonoBehaviour {
    private GameObject player, start, hint1, hint2, hint3, end;
    public GameObject room, street;

    void Start() {
        player = GameObject.FindWithTag("Player");

        // Street
        street.SetActive(false);


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

    public void ShowControls1() {
        hint1.SetActive(true);
        start.SetActive(false);
    }

    // Show hints

    public void ShowControls2() {
        hint1.SetActive(false);
        hint2.SetActive(true);
    }

    public void ShowControls3() {
        hint2.SetActive(false);
        hint3.SetActive(true);
    }

    public void ShowEndTutorial() {
        hint3.SetActive(false);
        end.SetActive(true);
    }

    public void EndTutorial() {
        end.SetActive(false);
        room.SetActive(false);
        street.SetActive(true);

        player.transform.position = new Vector3(0, 0.9f, 4.5f);

    }

    public void EndGame() {
        Application.Quit();
    }
}
