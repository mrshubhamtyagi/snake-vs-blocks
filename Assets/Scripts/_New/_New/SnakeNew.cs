using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNew : MonoBehaviour
{
    public enum MoveMethod { LerpOnly, CoroutineOnly, Both };

    public float moveSpeed = 1;
    public float turnSpeed = 1;
    public float rotateSpeed = 30;

    [Header("Lerp Only")]
    public float L_gap = 0.5f;
    public float L_lerp = 0.4f;

    [Header("Coroutine Only")]
    public float C_gap = 0.5f;
    public int C_skipFrames = 2;

    [Header("Both Lerp & Coroutine")]
    public float B_gap = 0.5f;
    public int B_skipframes = 2;
    public float B_lerp = 0.6f;

    [Space(10)] public MoveMethod moveMethod = MoveMethod.LerpOnly;

    private Rigidbody2D headRigidBody;
    private Vector3 velocity = Vector3.zero;
    private BodySpawnerNew bodySpawnerNew;

    private WaitForSeconds waitForSeconds;
    private WaitForEndOfFrame skipFrame = new WaitForEndOfFrame();
    private float _X;
    private float _Y;
    private float _Z;

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
        if (moveMethod == MoveMethod.LerpOnly)
            MoveBodyUsingOnlyLerp();
        if (moveMethod == MoveMethod.CoroutineOnly)
            MoveBodyUsingOnlyCoroutine();
        if (moveMethod == MoveMethod.Both)
            MoveBodyUsingBoth();
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
        }
    }
    #endregion

    #region Move Body Using Only Lerp
    private void MoveBodyUsingOnlyLerp()
    {
        if (bodySpawnerNew.listBody.Count > 1)
        {
            for (int i = 1; i < bodySpawnerNew.listBody.Count; i++)
            {
                Transform currentBodyPart = bodySpawnerNew.listBody[i];
                Transform previousBodyPart = bodySpawnerNew.listBody[i - 1];

                Vector3 _positionToFollow = previousBodyPart.position;
                _positionToFollow.y -= L_gap;

                currentBodyPart.position = Vector3.Lerp(currentBodyPart.position, _positionToFollow, L_lerp);
                currentBodyPart.rotation = Quaternion.Lerp(currentBodyPart.rotation, previousBodyPart.rotation, L_lerp);
            }
        }
    }
    #endregion

    #region Move Body Using Only Coroutine
    private void MoveBodyUsingOnlyCoroutine()
    {
        if (bodySpawnerNew.listBody.Count > 1)
        {
            for (int i = 1; i < bodySpawnerNew.listBody.Count; i++)
            {
                Transform currentBodyPart = bodySpawnerNew.listBody[i];
                Transform previousBodyPart = bodySpawnerNew.listBody[i - 1];

                Vector3 _positionToFollow = previousBodyPart.position;
                _positionToFollow.y -= L_gap;

                StartCoroutine(Co_MoveBodyUsingOnlyCoroutine(currentBodyPart, previousBodyPart, i));
            }
        }
    }
    private IEnumerator Co_MoveBodyUsingOnlyCoroutine(Transform _current, Transform _previous, int _index)
    {
        Vector3 _positionToFollow = _previous.position;
        _positionToFollow.y -= C_gap;

        _X = _current.position.x;
        _Y = _positionToFollow.y;
        _Z = _previous.position.z;
        _current.position = new Vector3(_X, _Y, _Z);

        if (_index == 1)
            yield return skipFrame;
        else
            for (int j = 0; j < C_skipFrames; j++)
                yield return skipFrame;

        _X = _positionToFollow.x;
        _Y = _current.position.y;
        _Z = _positionToFollow.z;
        _current.GetComponent<Rigidbody2D>().AddForce(Vector2.right.normalized * Input.GetAxis("Horizontal"), ForceMode2D.Impulse);
        //_current.position = new Vector3(_X, _Y, _Z);
        _current.rotation = Quaternion.Lerp(_current.rotation, _previous.rotation, L_lerp);

    }
    #endregion

    #region Move Body Using Both Lerp & Coroutine
    private void MoveBodyUsingBoth()
    {
        if (bodySpawnerNew.listBody.Count > 1)
        {
            Transform current;
            Transform previous;
            for (int i = 1; i < bodySpawnerNew.listBody.Count; i++)
            {
                current = bodySpawnerNew.listBody[i];
                previous = bodySpawnerNew.listBody[i - 1];

                StartCoroutine(Co_MoveBodyUsingBoth(current, previous, i));
            }
        }
    }

    private IEnumerator Co_MoveBodyUsingBoth(Transform _current, Transform _previous, int _index)
    {
        Vector3 _positionToFollow = _previous.position;
        _positionToFollow.y -= B_gap;

        _X = _current.position.x;
        _Y = Mathf.SmoothStep(_current.position.y, _positionToFollow.y, B_lerp);
        _Z = _previous.position.z;
        _current.position = new Vector3(_X, _Y, _Z);

        if (_index == 1)
            yield return skipFrame;
        else
            for (int j = 0; j < B_skipframes; j++)
                yield return skipFrame;

        _X = Mathf.SmoothStep(_current.position.x, _positionToFollow.x, B_lerp);
        _Y = _current.position.y;
        _Z = _previous.position.z;
        _current.position = new Vector3(_X, _Y, _Z);
        _current.rotation = Quaternion.Lerp(_current.rotation, _previous.rotation, B_lerp);

    }
    #endregion

}

