using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public enum GameState { MAIN_MENU, TUTORIAL1, TUTORIAL2, TUTORIAL3, TUTORIAL_DONE, MAIN_LEVEL, BOSS, ENDGAME };
    public GameState gameState { get; private set; }
    public List<GameState> tutorialStates = new List<GameState>();
    public int playerHealth;
    public int tutorialCounter, minCounter;

    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        return (_instance == null) ? new GameManager() : _instance;
    }

    private GameManager()
    {
        playerHealth = 20;
        tutorialCounter = 0;
        // Precisa fazer cada poder duas vezes
        minCounter = 2;
        gameState = GameState.MAIN_MENU;
        tutorialStates.Add(GameManager.GameState.TUTORIAL1);
        tutorialStates.Add(GameManager.GameState.TUTORIAL2);
        tutorialStates.Add(GameManager.GameState.TUTORIAL3);
        tutorialStates.Add(GameManager.GameState.TUTORIAL_DONE);
    }

    public delegate void ChangeStateDelegate();

    public void ChangeState(GameState nextState)
    {
        if (nextState == GameState.MAIN_LEVEL || nextState == GameState.BOSS)
        {
            ResetHealth();
        }
        gameState = nextState;
    }

    private void ResetHealth()
    {
        playerHealth = 20;
    }

    public void ResetCounter()
    {
        tutorialCounter = 0;
    }
}
