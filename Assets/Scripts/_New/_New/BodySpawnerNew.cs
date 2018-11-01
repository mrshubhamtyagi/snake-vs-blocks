using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySpawnerNew : MonoBehaviour
{
    public Transform bodyPrefab;
    public List<Transform> listBody = new List<Transform>(0);

    private SnakeNew snake;
    private bool firstTime = true;

    private void Awake()
    {
        snake = FindObjectOfType<SnakeNew>();
    }

    void Start()
    {
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();
        SpawnBody();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnBody();
    }

    public void SpawnBody()
    {
        Transform body;
        if (firstTime)
        {
            firstTime = false;
            body = Instantiate(bodyPrefab, Vector3.zero, Quaternion.identity);
            listBody.Add(body);
            body.SetParent(transform);
            snake.SetAsHead(body);
        }
        else
        {
            body = Instantiate(bodyPrefab, GetPosition(), Quaternion.identity);
            listBody.Add(body);
            body.SetParent(transform);
            body.tag = "BodyPoint";
        }

    }

    private Vector3 GetPosition()
    {
        Vector3 position = Vector3.zero;
        position.y = listBody[listBody.Count - 1].position.y;
        position.y -= 10;
        position.x = listBody[listBody.Count - 1].position.x;
        position.z = listBody[listBody.Count - 1].position.z;
        return position;
    }
}
