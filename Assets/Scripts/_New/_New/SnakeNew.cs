using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNew : MonoBehaviour
{
    public Transform bodyPrefab;
    public float moveSpeed = 1;
    public float turnSpeed = 1;
    public float rotateSpeed = 30;

    private List<Transform> listBody = new List<Transform>(0);
    private Rigidbody2D rb;

    Vector3 velocity = Vector3.zero;

    void Start()
    {
        Transform head = Instantiate(bodyPrefab, Vector3.zero, Quaternion.identity);
        listBody.Add(head);
        rb = head.GetComponent<Rigidbody2D>();
        head.SetParent(transform);
        CameraFollow.INSTANCE.target = head;
        head.tag = "Head";
    }

    private void Update()
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
