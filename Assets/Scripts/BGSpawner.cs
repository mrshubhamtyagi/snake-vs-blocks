using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawner : MonoBehaviour
{
    public Transform backgroundPrefab;
    public Transform bgParent;
    public float offsetInY;

    private float lastYPosition = 5f;

    void Start()
    {

    }

    public void SpawnBG()
    {
        StartCoroutine(Co_SpawnBG());
    }

    private IEnumerator Co_SpawnBG()
    {
        // Spawning BG using Pool
        Transform bg;
        bg = null;
        bool isBgAvailable = false;
        string status = "New BG is Spawned";

        yield return new WaitForEndOfFrame();
        for (int i = 0; i < bgParent.childCount; i++)
        {
            if (!bgParent.GetChild(i).gameObject.activeSelf)
            {
                bg = bgParent.GetChild(i); // Spawn from pool
                bg.gameObject.SetActive(true);
                isBgAvailable = true;
                status = "BG is Re-Used";
            }
        }

        if (!isBgAvailable)
            bg = Instantiate(backgroundPrefab); // Create new BG

        bg.SetParent(bgParent);
        bg.transform.position = new Vector2(0, offsetInY + lastYPosition);
        lastYPosition = bg.transform.position.y;
        print(status);
    }
}
