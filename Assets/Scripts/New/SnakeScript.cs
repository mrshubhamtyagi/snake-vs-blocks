using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
    public float turnSpeed = 4;
    public float moveSpeed = 2f;

    private Vector3 targetPosition;

    void Start()
    {

    }


    void Update()
    {
        targetPosition.x = Input.GetAxis("Horizontal") * turnSpeed;
        transform.Translate((Vector3.up * moveSpeed + targetPosition) * Time.deltaTime);
    }
}
