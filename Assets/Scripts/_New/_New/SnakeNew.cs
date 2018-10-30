using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNew : MonoBehaviour
{
    public Transform bodyPrefab;
    public float moveSpeed = 1;
    public float turnSpeed = 1;
    public float rotateSpeed = 30;

    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private BodySpawnerNew bodySpawnerNew;

    private void Awake()
    {
        bodySpawnerNew = FindObjectOfType<BodySpawnerNew>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        velocity.y = moveSpeed * Time.fixedDeltaTime;
        velocity.x = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;

        rb.velocity = velocity;
        rb.MoveRotation(Input.GetAxis("Horizontal") * -rotateSpeed);
        //rb.velocity = (Vector3.up * Time.fixedDeltaTime).normalized * moveSpeed;
    }


}
