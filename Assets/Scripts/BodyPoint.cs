using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPoint : MonoBehaviour
{
    private BodyPointSpawner pointSpawner;

    private Vector3 finalPosition;
    private static float bodyPointLerp = 0.3f;

    #region SETTERS AND GETTERS
    public void SetFinalPosition(Vector3 position)
    {
        finalPosition = position;
    }
    public void SetBodyPointLerp(float value)
    {
        bodyPointLerp = value;
    }
    #endregion

    private void Awake()
    {
        if (FindObjectOfType<BodyPointSpawner>())
            pointSpawner = FindObjectOfType<BodyPointSpawner>();
        else
            Debug.LogError("SnakeBodySpawner is Missing");
    }

    void Start()
    {
        finalPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, bodyPointLerp);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Snake"))
        {
            pointSpawner.AddBodyToList(this);
            transform.name = pointSpawner.GetSnakeLength().ToString();
            transform.SetParent(FindObjectOfType<Snake>().transform);
            Vector3 _pos = GetNextPosition();
            transform.position = _pos;
            GetComponent<CircleCollider2D>().isTrigger = false;
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
        if (pointSpawner.GetSnakeLength() > 0)
        {
            if (pointSpawner.GetSnakeLength() == 1)
            {
                position = Vector3.zero;
            }
            else
            {
                if (pointSpawner.GetElementAtIndex(pointSpawner.GetSnakeLength() - 2) != null)
                {
                    position = pointSpawner.GetElementAtIndex(pointSpawner.GetSnakeLength() - 2).transform.position;
                    position.y -= FindObjectOfType<Snake>().GetBodyGap();
                }
            }
        }
        else
            Debug.LogError("Length is 0");

        return position;
    }
}
