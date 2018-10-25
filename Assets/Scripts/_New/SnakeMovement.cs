﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public Transform bodyPrefab;
    public float moveSpeed = 2;
    public float turnSpeed = 4;
    public float rotateSpeed = 10;
    public float lerpX = 0.2f;
    public float lerpY = 0.3f;
    public float gap = 0.5f;
    public int waitFrames = 4;
    public float rotationLerp = 0.4f;
    public bool freezeRotation = true;

    //--------------------------------------------------- PRIVATE FIELDS
    private bool firstTime = true;
    [SerializeField] private List<Transform> bodyList = new List<Transform>();
    private float turnAxis;
    private Quaternion headRotation;

    void Start()
    {
        for (int i = 0; i < 8; i++)
            SpawnBody();
    }

    void Update()
    {
        turnAxis = Input.GetAxis("Horizontal");
        headRotation = Quaternion.Euler(0, 0, rotateSpeed * -turnAxis);
        StartCoroutine("Co_Move");
        print(turnAxis);
    }

    private IEnumerator Co_Move()
    {
        // --------------------------------- HEAD
        if (bodyList.Count > 0)
        {
            bodyList[0].Translate(Vector3.up * Time.smoothDeltaTime * moveSpeed, Space.World);
            bodyList[0].Translate(Vector3.right * Time.smoothDeltaTime * turnAxis * turnSpeed, Space.World);

            if (!freezeRotation)
                bodyList[0].rotation = Quaternion.Lerp(bodyList[0].rotation, headRotation, rotationLerp);
        }

        // --------------------------------- OTHER BODY PARTS
        if (bodyList.Count > 1)
        {
            Vector3 targetPosition;

            for (int i = 1; i < bodyList.Count; i++)
            {
                targetPosition = bodyList[i - 1].position;
                targetPosition.y -= gap;

                //|| turnAxis > .99f || turnAxis < -.99f
                if (turnAxis == 0f)
                    for (int w = 0; w < waitFrames; w++)
                        yield return new WaitForEndOfFrame();
                else
                    for (int w = 0; w < waitFrames - (waitFrames * .40f); w++)
                        yield return new WaitForEndOfFrame();

                bodyList[i].position = targetPosition;
                bodyList[i].rotation = bodyList[i - 1].rotation;
            }
        }
    }

    private void SpawnBody()
    {
        Transform body;

        if (firstTime)
        {
            firstTime = false;
            body = Instantiate(bodyPrefab, Vector3.zero, Quaternion.identity);
            body.name = "Head";
            CameraFollow.INSTANCE.target = body;
            body.tag = "Head";
        }
        else
        {
            Vector3 positionToSpawn = bodyList[bodyList.Count - 1].position;
            positionToSpawn.y -= 10f;

            body = Instantiate(bodyPrefab, positionToSpawn, Quaternion.identity);
            body.name = "Body";
        }

        bodyList.Add(body);
    }

    private void OnDisable()
    {
        bodyList.Clear();
    }
}
