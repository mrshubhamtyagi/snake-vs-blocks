using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float followSpeed = 0.4f;

    void Start()
    {
        transform.position = targetOffset;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + targetOffset;
            targetPosition.x = 0f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);

        }
    }
}
