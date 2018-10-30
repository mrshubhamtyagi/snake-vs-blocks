using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNew : MonoBehaviour
{
    public float moveSpeed = 1;
    public float turnSpeed = 1;
    public float rotateSpeed = 30;
    public float waitTime = 0.1f;
    public float followSpeed = 0.4f;
    public float gap = 0.5f;
    public bool moveUsingLerp = true;

    private Rigidbody2D headRigidBody;
    private Vector3 velocity = Vector3.zero;
    private BodySpawnerNew bodySpawnerNew;

    private WaitForSeconds waitForSeconds;
    private WaitForEndOfFrame skipFrame = new WaitForEndOfFrame();
    private Vector3 positionToFollow;
    private Quaternion rotationToFollow;

    public void SetAsHead(Transform body)
    {
        headRigidBody = body.GetComponent<Rigidbody2D>();
        CameraFollow.INSTANCE.target = body;
        body.tag = "Head";
        body.name = "Head";
    }

    private void Awake()
    {
        bodySpawnerNew = FindObjectOfType<BodySpawnerNew>();
    }

    void Start()
    {
        waitForSeconds = new WaitForSeconds(waitTime);
    }

    void FixedUpdate()
    {
        StartCoroutine(Co_Move());
    }

    private IEnumerator Co_Move()
    {
        if (headRigidBody != null)
        {
            velocity.y = moveSpeed * Time.fixedDeltaTime;
            velocity.x = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;

            headRigidBody.velocity = velocity;
            headRigidBody.MoveRotation(Input.GetAxis("Horizontal") * -rotateSpeed);
            //rb.velocity = (Vector3.up * Time.fixedDeltaTime).normalized * moveSpeed;
        }

        if (bodySpawnerNew.listBody.Count > 1)
        {
            for (int i = 1; i < bodySpawnerNew.listBody.Count; i++)
            {
                Transform currentBodyPart = bodySpawnerNew.listBody[i];

                positionToFollow = bodySpawnerNew.listBody[i - 1].position;
                positionToFollow.y -= gap;
                rotationToFollow = bodySpawnerNew.listBody[i - 1].rotation;

                #region USING LERP
                if (moveUsingLerp)
                {
                    currentBodyPart.position = Vector3.Lerp(currentBodyPart.position, positionToFollow, followSpeed);
                    currentBodyPart.rotation = Quaternion.Lerp(currentBodyPart.rotation, rotationToFollow, followSpeed);
                }
                #endregion

                #region USING WAIT FOR SECONDS
                if (!moveUsingLerp)
                {
                    for (int f = 0; f < waitTime; f++)
                        yield return skipFrame;

                    //positionToFollow = bodySpawnerNew.listBody[i - 1].position;
                    //positionToFollow.y -= gap;
                    //rotationToFollow = bodySpawnerNew.listBody[i - 1].rotation;

                    //currentBodyPart.position = Vector3.Lerp(currentBodyPart.position, positionToFollow, followSpeed * 2);
                    //currentBodyPart.rotation = Quaternion.Lerp(currentBodyPart.rotation, rotationToFollow, followSpeed * 2);
                    currentBodyPart.position = positionToFollow;
                    currentBodyPart.rotation = rotationToFollow;

                }
                #endregion
            }
        }
    }

}

