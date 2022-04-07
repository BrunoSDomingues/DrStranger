using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public GameObject prefab1, prefab2;
    private GameObject wall, tempCrate, hint1, canvas;
    private List<GameObject> crates = new List<GameObject>();
    private bool loaded = false;

    // Start is called before the first frame update
    void Start()
    {
        wall = Instantiate(prefab1, new Vector3(0, 0.95f, -2.0f), Quaternion.identity);
        hint1 = GameObject.FindWithTag("Hint1");
        hint1.SetActive(false);
        canvas = GameObject.Find("InteractableCanvas");
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
            tempCrate = Instantiate(prefab2, new Vector3(4 * (1 - i), 0.8f, -7f), Quaternion.identity);
            crates.Add(tempCrate);
        }
        MoveWall(-12f);
        loaded = true;
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Crate").Length == 0 && loaded) Debug.Log("First power done!");
    }
}
