using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public GameObject wallPrefab, cratePrefab, turretPrefab;
    private GameObject wall, tempCrate, canvas, hint1, hint2, hint3, turret;
    private List<GameObject> crates = new List<GameObject>(), turrets = new List<GameObject>();
    private bool loadedCrates = false, loadedTurret = false, loadedTurrets = false;

    // Start is called before the first frame update
    void Start()
    {
        wall = Instantiate(wallPrefab, new Vector3(0, 0.95f, -2.0f), Quaternion.identity);
        hint1 = GameObject.FindWithTag("Hint1");
        hint1.SetActive(false);
        hint2 = GameObject.FindWithTag("Hint2");
        hint2.SetActive(false);
        hint3 = GameObject.FindWithTag("Hint3");
        hint3.SetActive(false);
        canvas = GameObject.FindWithTag("Canvas");
    }

    public void StartGame()
    {
        MoveWall(-3.0f);
        hint1.SetActive(true);
        canvas.SetActive(false);
    }

    void MoveWall(float z)
    {
        wall.transform.position = new Vector3(0, 0.95f, z);
    }

    public void LoadCrates()
    {
        GameObject.FindWithTag("Hint1").SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            tempCrate = Instantiate(cratePrefab, new Vector3(4 * (1 - i), 0.85f, -7f), Quaternion.identity);
            crates.Add(tempCrate);
        }
        MoveWall(-12f);
        loadedCrates = true;
    }

    public void LoadTurret()
    {
        hint2.SetActive(false);
        turret = Instantiate(turretPrefab, new Vector3(-1.8f, 0.9f, -24f), Quaternion.identity);
        MoveWall(-28f);
        loadedTurret = true;
    }

    void CheckFirst()
    {
        if (GameObject.FindGameObjectsWithTag("Crate").Length == 0 && loadedCrates && !loadedTurret && !loadedTurrets)
        {
            Debug.Log("First power done!");
            MoveWall(-13f);
            hint2.SetActive(true);
        }
    }

    void CheckSecond()
    {
        if (loadedTurret && !loadedTurrets && ShieldCollision.blockedShots >= 3)
        {
            Debug.Log("Second power done!");
            MoveWall(-32f);
            hint3.SetActive(true);
        }
    }

    void Update()
    {
        CheckFirst();
        CheckSecond();
    }
}
