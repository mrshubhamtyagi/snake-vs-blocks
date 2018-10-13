using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private SnakeBodySpawner bodySpawner;

    private void Awake()
    {
        if (FindObjectOfType<SnakeBodySpawner>())
            bodySpawner = FindObjectOfType<SnakeBodySpawner>();
        else
            Debug.LogError("SnakeBodySpawner is Missing");
    }

    void Start()
    {

    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("SnakeBody"))
        {
            print("Collided");
            col.transform.SetParent(FindObjectOfType<Snake>().transform);
            col.transform.position = GetNextPosition();
            if (col.gameObject.GetComponent<SnakeBody>())
                bodySpawner.AddBodyToList(col.gameObject.GetComponent<SnakeBody>());
            else
                Debug.LogError("SnakeBodyScript is Missing");
        }
    }

    private Vector3 GetNextPosition()
    {
        Vector3 position = bodySpawner.GetLastBodyFromList().transform.position;
        position.y -= FindObjectOfType<Snake>().GetSnakeBodyGap();

        return position;
    }
}
