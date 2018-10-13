using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public float turnSpeed = 10f;
    public float moveSpeed = 10f;

    private Vector3 position;

    void Start()
    {

    }

    void Update()
    {
        position.x = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        position.y = moveSpeed * Time.deltaTime;
        position.z = 0f;

        Move();
    }

    private void Move()
    {
        position.x = Mathf.Clamp(position.x, -2.5f, 2.5f);
        transform.Translate(position);
    }
}
