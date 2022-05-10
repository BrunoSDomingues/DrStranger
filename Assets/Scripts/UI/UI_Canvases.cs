using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Canvases : MonoBehaviour
{
    GameManager gm;
    private GameObject hint1, hint2, hint3, hint4, start;
    

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }

    void Start()
    {
        // Main Menu
        start = GameObject.FindWithTag("Canvas");

        // Tutorial hints
        hint1 = GameObject.FindWithTag("Hint1");
        hint1.SetActive(false);
        hint2 = GameObject.FindWithTag("Hint2");
        hint2.SetActive(false);
        hint3 = GameObject.FindWithTag("Hint3");
        hint3.SetActive(false);
        hint4 = GameObject.FindWithTag("Hint4");
        hint4.SetActive(false);
    }

    public void StartGame()
    {
        hint1.SetActive(true);
        start.SetActive(false);
        gm.ChangeState(GameManager.GameState.TUTORIAL1);
    }

    public void TestFirst()
    {
        hint1.SetActive(false);
    }

    public void TestSecond()
    {
        hint2.SetActive(false);
    }

    public void TestThird()
    {
        hint3.SetActive(false);
    }

    void CheckFirst()
    {
        if (gm.tutorialCounter >= gm.minCounter && gm.gameState == GameManager.GameState.TUTORIAL1)
        {
            Debug.Log("First power done!");
            hint2.SetActive(true);
            gm.ChangeState(GameManager.GameState.TUTORIAL2);
            gm.ResetCounter();
        }
    }
    
    void CheckSecond()
    {
        if (gm.tutorialCounter >= gm.minCounter && gm.gameState == GameManager.GameState.TUTORIAL2)
        {
            Debug.Log("Second power done!");
            hint3.SetActive(true);
            gm.ChangeState(GameManager.GameState.TUTORIAL3);
            gm.ResetCounter();
        }
    }

    void CheckThird()
    {
        if (gm.tutorialCounter >= gm.minCounter && gm.gameState == GameManager.GameState.TUTORIAL3)
        {
            Debug.Log("Third power done!");
            hint4.SetActive(true);
            gm.ChangeState(GameManager.GameState.TUTORIAL_DONE);
            gm.ResetCounter();
        }
    }

    public void EndTutorial() {
        gm.ChangeState(GameManager.GameState.MAIN_LEVEL);
    }

    void Update()
    {
        if (gm.tutorialStates.Contains(gm.gameState))
        {
            CheckFirst();
            CheckSecond();
            CheckThird();
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
