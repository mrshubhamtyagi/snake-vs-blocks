using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodySpawner : MonoBehaviour
{
    public SnakeBody snakeBodyPrefab;
    public Vector2 spawnXRange;
    public Vector2 spawnYRange;
    public Transform snakeBodiesParent;


    private List<SnakeBody> snakeBodiesList = new List<SnakeBody>();
    private Vector3 lastSpawnedPosition;

    #region SETTERS AND GETTER
    public List<SnakeBody> GetBosyList()
    {
        return snakeBodiesList;
    }
    public SnakeBody GetLastBodyFromList()
    {
        return snakeBodiesList[snakeBodiesList.Count - 1];
    }
    public int GetSnakeLength()
    {
        return snakeBodiesList.Count;
    }
    public void AddBodyToList(SnakeBody body)
    {
        snakeBodiesList.Add(body);
        //body.transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
    #endregion

    void Start()
    {
        // Head
        SnakeBody head = SpawnSnakeBodyFromPool();
        if (head != null)
        {
            head.name = "Head";
            head.transform.SetParent(FindObjectOfType<Snake>().transform);
            head.transform.position = Vector3.zero;
            snakeBodiesList.Add(head);
        }

        for (int i = 0; i < 10; i++)
            SpawnSnakeBodyFromPool();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnSnakeBodyFromPool();
        }
    }

    private SnakeBody SpawnSnakeBodyFromPool()
    {
        SnakeBody body = null;
        bool isBodyFound = false;

        // first look if any snake body is available
        if (snakeBodiesParent != null)
        {
            if (snakeBodiesParent.childCount > 0)
            {
                for (int i = 0; i < snakeBodiesParent.childCount; i++)
                {
                    if (!snakeBodiesParent.GetChild(i).gameObject.activeSelf)
                    {
                        if (snakeBodiesParent.GetChild(i).GetComponent<SnakeBody>())
                        {
                            // assign snake body
                            body = snakeBodiesParent.GetChild(i).GetComponent<SnakeBody>();
                            isBodyFound = true;
                        }
                        else
                            Debug.LogError("Snake Body Script is Missing");
                    }
                }
            }

            //if no snake body is available
            if (!isBodyFound)
                body = Instantiate(snakeBodyPrefab);

            // basic body setup
            body.gameObject.SetActive(true);
            body.transform.SetParent(snakeBodiesParent);
            body.gameObject.name = "SnakeBody";
            body.transform.rotation = Quaternion.identity;
            body.transform.position = GetSnaleBodyPosition();
        }
        else
            Debug.LogError("SnakeBodyParent is Missing");

        return body;
    }

    private Vector3 GetSnaleBodyPosition()
    {
        Vector3 position = Vector3.zero;

        position.x = UnityEngine.Random.Range(spawnXRange.x, spawnXRange.y);
        position.y = lastSpawnedPosition.y + UnityEngine.Random.Range(spawnYRange.x, spawnYRange.y);
        position.z = 0;

        lastSpawnedPosition = position;
        return position;
    }
}
