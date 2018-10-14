using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPointSpawner : MonoBehaviour
{
    public BodyPoint bodyPointPrefab;
    public Vector2 spawnXRange;
    public Vector2 spawnYRange;
    public Transform bodyPointsParent;


    private List<BodyPoint> snakeBodyPointList = new List<BodyPoint>();
    private Vector3 lastSpawnedPosition;

    #region SETTERS AND GETTER
    public List<BodyPoint> GetCompleteList()
    {
        return snakeBodyPointList;
    }
    public BodyPoint GetLastElement()
    {
        return snakeBodyPointList[snakeBodyPointList.Count - 1];
    }
    public BodyPoint GetElementAtIndex(int index)
    {
        if (index < snakeBodyPointList.Count)
            return snakeBodyPointList[index];
        else
            return null;

    }
    public int GetSnakeLength()
    {
        return snakeBodyPointList.Count;
    }
    public void AddBodyToList(BodyPoint body)
    {
        snakeBodyPointList.Add(body);
        //body.transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
    public void RemoveFromTheList(BodyPoint point)
    {
        snakeBodyPointList.Remove(point);
    }
    #endregion

    void Start()
    {
        // Head Setup
        BodyPoint head = SpawnSnakeBodyFromPool();
        if (head != null)
        {
            head.name = "Head";
            head.transform.SetParent(FindObjectOfType<Snake>().transform);
            head.transform.position = Vector3.zero;
            //snakeBodiesList.Add(head);
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

    private BodyPoint SpawnSnakeBodyFromPool()
    {
        BodyPoint point = null;
        bool isPointFound = false;

        // first look if any snake body is available
        if (bodyPointsParent != null)
        {
            if (bodyPointsParent.childCount > 0)
            {
                for (int i = 0; i < bodyPointsParent.childCount; i++)
                {
                    if (!bodyPointsParent.GetChild(i).gameObject.activeSelf)
                    {
                        if (bodyPointsParent.GetChild(i).GetComponent<BodyPoint>())
                        {
                            // assign snake body
                            point = bodyPointsParent.GetChild(i).GetComponent<BodyPoint>();
                            isPointFound = true;
                        }
                        else
                            Debug.LogError("SnakeBodyPoint Script is Missing");
                    }
                }
            }

            //if no snake body is available
            if (!isPointFound)
                point = Instantiate(bodyPointPrefab);

            // basic body setup
            point.gameObject.SetActive(true);
            point.transform.SetParent(bodyPointsParent);
            point.gameObject.name = "BodyPoint";
            point.transform.rotation = Quaternion.identity;
            point.transform.position = GetNextBodyPointPosition();
        }
        else
            Debug.LogError("SnakeBodyParent is Missing");

        return point;
    }

    private Vector3 GetNextBodyPointPosition()
    {
        Vector3 position = Vector3.zero;

        position.x = UnityEngine.Random.Range(spawnXRange.x, spawnXRange.y);
        position.y = lastSpawnedPosition.y + UnityEngine.Random.Range(spawnYRange.x, spawnYRange.y);
        position.z = 0;

        lastSpawnedPosition = position;
        return position;
    }
}
