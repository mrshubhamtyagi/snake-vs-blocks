using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNew : MonoBehaviour
{
    public enum MoveMethod { Lerp, Coroutine };

    public float moveSpeed = 1;
    public float turnSpeed = 1;
    public float rotateSpeed = 30;

    [Header("Lerp Fiels")]
    public float lerpGap = 0.5f;
    public float followLerp = 0.4f;

    [Header("Lerp & Coroutine Fiels")]
    public float coroutineGap = 0.5f;
    public int skipFrames = 2;
    public float coroutineLerp = 0.6f;

    [Space(10)] public MoveMethod moveMethod = MoveMethod.Lerp;

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
        waitForSeconds = new WaitForSeconds(0.5f);
    }

    void FixedUpdate()
    {
        MoveHead();
        if (moveMethod == MoveMethod.Lerp)
            MoveBodyUsingLerp();
        if (moveMethod == MoveMethod.Coroutine)
            MoveBodyUsingCoroutine();
    }

    #region Move Head
    private void MoveHead()
    {

        if (headRigidBody != null)
        {
            velocity.y = moveSpeed * Time.fixedDeltaTime;
            velocity.x = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;

            headRigidBody.velocity = velocity;
            headRigidBody.MoveRotation(Input.GetAxis("Horizontal") * -rotateSpeed);
            //rb.velocity = (Vector3.up * Time.fixedDeltaTime).normalized * moveSpeed;
        }
    }
    #endregion

    #region Move Body Using Lerp
    private void MoveBodyUsingLerp()
    {
        if (bodySpawnerNew.listBody.Count > 1)
        {
            for (int i = 1; i < bodySpawnerNew.listBody.Count; i++)
            {
                Transform currentBodyPart = bodySpawnerNew.listBody[i];
                Transform previousBodyPart = bodySpawnerNew.listBody[i - 1];

                positionToFollow = previousBodyPart.position;
                positionToFollow.y -= lerpGap;
                rotationToFollow = previousBodyPart.rotation;

                currentBodyPart.position = Vector3.Lerp(currentBodyPart.position, positionToFollow, followLerp);
                currentBodyPart.rotation = Quaternion.Lerp(currentBodyPart.rotation, rotationToFollow, followLerp);
            }
        }
    }
    #endregion

    #region Move Body Using Coroutine
    private void MoveBodyUsingCoroutine()
    {
        if (bodySpawnerNew.listBody.Count > 1)
        {
            Transform current;
            Transform previous;
            for (int i = 1; i < bodySpawnerNew.listBody.Count; i++)
            {
                current = bodySpawnerNew.listBody[i];
                previous = bodySpawnerNew.listBody[i - 1];
                StartCoroutine(Co_MoveBodyUsingCoroutine(current, previous));
            }
        }
    }

    private IEnumerator Co_MoveBodyUsingCoroutine(Transform _current, Transform _previous)
    {
        positionToFollow = _previous.position;
        positionToFollow.y -= coroutineGap;
        rotationToFollow = _previous.rotation;

        float _X = _current.position.x;
        float _Y = Mathf.SmoothStep(_current.position.y, positionToFollow.y, coroutineLerp);
        float _Z = _previous.position.z;
        _current.position = new Vector3(_X, _Y, _Z);

        for (int j = 0; j < skipFrames; j++)
            yield return skipFrame;

        _X = Mathf.SmoothStep(_current.position.x, positionToFollow.x, coroutineLerp);
        _Y = _current.position.y;
        _Z = _previous.position.z;
        _current.position = new Vector3(_X, _Y, _Z);
        _current.rotation = Quaternion.Lerp(_current.rotation, _previous.rotation, coroutineLerp);

    }
    #endregion

}

