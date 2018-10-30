using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySpawnerNew : MonoBehaviour
{
    public Transform bodyPrefab;
    public float gap;

    public List<Transform> listBody = new List<Transform>(0);

    private bool firstTime = true;

    void Start()
    {

    }

    void Update()
    {

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
            CameraFollow.INSTANCE.target = body;
            body.tag = "Head";
        }
        else
        {
            body = Instantiate(bodyPrefab, GetPosition(), Quaternion.identity);
            listBody.Add(body);
            body.SetParent(transform);
            body.tag = "Body";
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
