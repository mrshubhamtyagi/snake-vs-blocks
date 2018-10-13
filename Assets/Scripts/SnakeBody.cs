using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private SnakeBodySpawner bodySpawner;

    private Vector3 finalPosition;
    private float bodyAnimSpeed = 0.3f;
    private bool isAlreadyAPartOfSnake = false;

    #region SETTERS AND GETTERS
    public void SetFinalPosition(Vector3 position)
    {
        finalPosition = position;
    }
    public void SetBodyAnimSpeed(float speed)
    {
        bodyAnimSpeed = speed;
    }
    #endregion

    private void Awake()
    {
        if (FindObjectOfType<SnakeBodySpawner>())
            bodySpawner = FindObjectOfType<SnakeBodySpawner>();
        else
            Debug.LogError("SnakeBodySpawner is Missing");
    }

    void Start()
    {
        finalPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, bodyAnimSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Snake"))
        {
            bodySpawner.AddBodyToList(this);
            transform.name = bodySpawner.GetSnakeLength().ToString();
            transform.SetParent(FindObjectOfType<Snake>().transform);
            Vector3 _pos = GetNextPosition();
            transform.position = _pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        //if (col.transform.CompareTag("SnakeBody"))
        //{
        //    print("Collided");
        //    col.transform.SetParent(FindObjectOfType<Snake>().transform);
        //    col.transform.position = GetNextPosition();
        //    if (col.gameObject.GetComponent<SnakeBody>())
        //    {
        //        col.gameObject.name = bodySpawner.GetSnakeLength().ToString();
        //        bodySpawner.AddBodyToList(col.gameObject.GetComponent<SnakeBody>());
        //        col.transform.SetSiblingIndex(col.transform.childCount - 1);
        //    }
        //    else
        //        Debug.LogError("SnakeBodyScript is Missing");
        //}
    }

    private Vector3 GetNextPosition()
    {
        Vector3 position = Vector3.zero;
        if (bodySpawner.GetSnakeLength() > 0)
        {
            if (bodySpawner.GetSnakeLength() == 1)
            {
                position = Vector3.zero;
            }
            else
            {
                if (bodySpawner.GetElementAtIndex(bodySpawner.GetSnakeLength() - 2) != null)
                {
                    position = bodySpawner.GetElementAtIndex(bodySpawner.GetSnakeLength() - 2).transform.position;
                    position.y -= FindObjectOfType<Snake>().GetSnakeBodyGap();
                }
            }
        }
        else
            Debug.LogError("Length is 0");

        return position;
    }
}
