using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public GameObject /*wallPrefab,*/ cratePrefab, turretPrefab, wallLevel0;
    private GameObject player, tempCrate, canvas, hint1, hint2, hint2_5, hint3, hint4, endGame, turret, tempTurret /*, wall*/;
    private List<GameObject> crates = new List<GameObject>(), turrets = new List<GameObject>();
    private bool loadedCrates = false, loaded2_5 = false, loadedTurret = false;
    public static bool winGame = false, loadedTurrets = false, endTutorial = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //wall = Instantiate(wallPrefab, new Vector3(0, 0.95f, -2.0f), Quaternion.identity);
        hint1 = GameObject.FindWithTag("Hint1");
        hint1.SetActive(false);
        hint2 = GameObject.FindWithTag("Hint2");
        hint2.SetActive(false);
        hint2_5 = GameObject.FindWithTag("Hint2_5");
        hint2_5.SetActive(false);
        hint3 = GameObject.FindWithTag("Hint3");
        hint3.SetActive(false);
        hint4 = GameObject.FindWithTag("Hint4");
        hint4.SetActive(false);
        endGame = GameObject.FindWithTag("End");
        endGame.SetActive(false);
        canvas = GameObject.FindWithTag("Canvas");
    }

    public void StartGame()
    {
        //MoveWall(-3.0f);
        hint1.SetActive(true);
        canvas.SetActive(false);
    }

    //void MoveWall(float z)
    //{
    //    wall.transform.position = new Vector3(0, 0.95f, z);
    //}

    public void LoadCrates()
    {
        GameObject.FindWithTag("Hint1").SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            tempCrate = Instantiate(cratePrefab, new Vector3(4 * (1 - i), 1.2f, 18f), Quaternion.identity);
            crates.Add(tempCrate);
        }
        //MoveWall(-12f);
        loadedCrates = true;
    }

    public void LoadTurret()
    {
        hint2_5.SetActive(false);
        turret = Instantiate(turretPrefab, new Vector3(-1.8f, 1.2f, 18f), Quaternion.identity);
        //MoveWall(-28f);
        loadedTurret = true;
    }

    public void LoadTurrets()
    {
        loadedTurrets = true;
        hint3.SetActive(false);
        StartCoroutine(Delay(1f));
        tempTurret = Instantiate(turretPrefab, new Vector3(2.5f, 0.9f, 18f), Quaternion.identity);
        turrets.Add(tempTurret);
        tempTurret = Instantiate(turretPrefab, new Vector3(-1.8f, 0.9f, 18f), Quaternion.identity);
        turrets.Add(tempTurret);
        tempTurret = Instantiate(turretPrefab, new Vector3(-6.5f, 0.9f, 18f), Quaternion.identity);
        turrets.Add(tempTurret);
        //MoveWall(-45f);
    }

    public void ShowHint2_5()
    {
        loaded2_5 = true;
        hint2.SetActive(false);
        hint2_5.SetActive(true);
    }

    void CheckFirst()
    {
        if (GameObject.FindGameObjectsWithTag("Crate").Length == 0 && loadedCrates && !loaded2_5 && !loadedTurret && !loadedTurrets)
        {
            Debug.Log("First power done!");
            //MoveWall(-13f);
            hint2.SetActive(true);
        }
    }

    void CheckSecond()
    {
        if (loadedTurret && !loadedTurrets && ShieldCollision.blockedShots >= 3)
        {
            Debug.Log("Second power done!");
            if (turret) Destroy(turret);
            //MoveWall(-34f);
            hint3.SetActive(true);
        }
    }

    void CheckThird()
    {
        if (loadedTurrets && winGame)
        {
            foreach (GameObject t in turrets) Destroy(t);
            hint4.SetActive(true);

            //MoveWall(-61f);
        }
    }

    public void EndTutorial() {
        player.transform.position = new Vector3(0, 1.5f, 8);
        Destroy(wallLevel0);
        hint4.SetActive(false);
        endTutorial = true;
    }

    public void EndGame() {
        Application.Quit();
    }

    void Update()
    {
        if (!endTutorial) {
            CheckFirst();
            CheckSecond();
            CheckThird();
        }
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }
}
