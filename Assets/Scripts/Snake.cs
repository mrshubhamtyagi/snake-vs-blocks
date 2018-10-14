using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float turnSpeed = 10f;
    public float moveSpeed = 10f;
    public int curveWaitTime = 3;
    public float bodyPointLerp;

    [Header("Body Stuff")]
    [SerializeField] private float bodyGap;

    private BodyPointSpawner pointSpawner;
    private BGSpawner bGSpawner;
    private Vector3 position;
    private WaitForSeconds bodyUpdateWait;

    #region SETTERS AND GETTERS
    public Vector2 GetSnakePosition()
    {
        return transform.position;
    }
    public float GetBodyGap()
    {
        return bodyGap;
    }
    #endregion

    private void Awake()
    {
        if (FindObjectOfType<BodyPointSpawner>())
            pointSpawner = FindObjectOfType<BodyPointSpawner>();
        else
            Debug.LogError("BodyPointSpawner Script is Missing");

        if (FindObjectOfType<BGSpawner>())
            bGSpawner = FindObjectOfType<BGSpawner>();
        else
            Debug.LogError("BGSpawner Script is Missing");
    }

    private void Start()
    {
        bodyUpdateWait = new WaitForSeconds(curveWaitTime);
        FindObjectOfType<BodyPoint>().SetBodyPointLerp(bodyPointLerp);
    }

    void Update()
    {
        GetInput();
        Move();
    }

    private void GetInput()
    {
        position.x = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        position.y = moveSpeed * Time.deltaTime;
        position.z = 0f;
    }

    private void Move()
    {
        position.x = Mathf.Clamp(position.x, -2.5f, 2.5f); // Clamping the x value
        transform.Translate(position);
        StartCoroutine(Co_UpdateSnakeBody()); // Update Body Parts
    }

    private IEnumerator Co_UpdateSnakeBody()
    {
        if (pointSpawner.GetSnakeLength() > 0)
        {
            Vector3 _position = Vector3.zero;
            if (pointSpawner.GetSnakeLength() == 1)
            {
                _position = GetSnakePosition();
                if (pointSpawner.GetElementAtIndex(0) != null)
                {
                    pointSpawner.GetElementAtIndex(0).SetFinalPosition(_position);
                }
                else
                    Debug.Log("Invalid index");
            }
            else
            {
                for (int i = 0; i < pointSpawner.GetSnakeLength(); i++)
                {
                    if (i == 0)
                    {
                        _position = GetSnakePosition();
                        if (pointSpawner.GetElementAtIndex(0) != null)
                        {
                            pointSpawner.GetElementAtIndex(0).SetFinalPosition(_position);
                        }
                    }
                    else if (pointSpawner.GetElementAtIndex(i - 1) != null)
                    {
                        _position = pointSpawner.GetElementAtIndex(i - 1).transform.position;
                        _position.y -= bodyGap;

                        // wait for 5 frames then update the finalposition
                        // we could also use waitforseconds but it will create jitter issue
                        for (int w = 0; w < curveWaitTime; w++)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        if (pointSpawner.GetElementAtIndex(i))
                            pointSpawner.GetElementAtIndex(i).SetFinalPosition(_position);
                        else
                            Debug.Log(string.Format("Element at {0} index is not found", i));
                    }
                    else
                        Debug.Log("Invalid index");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Regenerating and deactivating BG
        if (col.transform.CompareTag("BG"))
        {
            bGSpawner.SpawnBG();
            if (col.GetComponent<BG>())
            {
                col.GetComponent<BG>().DeactivateBG(8);
            }
            else
                Debug.LogError("BG Script in Missing");
        }
    }
}
