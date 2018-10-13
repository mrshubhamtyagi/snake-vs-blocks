using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float turnSpeed = 10f;
    public float moveSpeed = 10f;
    public float snakeAnimSpeed = 0.1f;

    [Header("Body Stuff")]
    [SerializeField] private float snakeBodyGap;

    private SnakeBodySpawner bodySpawner;
    private BGSpawner bGSpawner;
    private Vector3 position;
    private WaitForSeconds bodyUpdateWait;

    #region SETTERS AND GETTERS
    public Vector2 GetSnakePosition()
    {
        return transform.position;
    }
    public float GetSnakeBodyGap()
    {
        return snakeBodyGap;
    }
    #endregion

    private void Awake()
    {
        if (FindObjectOfType<SnakeBodySpawner>())
            bodySpawner = FindObjectOfType<SnakeBodySpawner>();
        else
            Debug.LogError("SnakeBodySpawner Script is Missing");

        if (FindObjectOfType<BGSpawner>())
            bGSpawner = FindObjectOfType<BGSpawner>();
        else
            Debug.LogError("BGSpawner Script is Missing");
    }

    private void Start()
    {
        bodyUpdateWait = new WaitForSeconds(snakeAnimSpeed);
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
        StartCoroutine(Co_UpdateSnakeBody()); // Update Body
    }

    private IEnumerator Co_UpdateSnakeBody()
    {
        if (bodySpawner.GetSnakeLength() > 0)
        {
            Vector3 _position = Vector3.zero;
            if (bodySpawner.GetSnakeLength() == 1)
            {
                yield return bodyUpdateWait;
                _position = GetSnakePosition();
                if (bodySpawner.GetElementAtIndex(0) != null)
                    bodySpawner.GetElementAtIndex(0).SetFinalPosition(_position);
                else
                    Debug.Log("Invalid index");
            }
            else
            {
                print(bodySpawner.GetSnakeLength());

                for (int i = 0; i < bodySpawner.GetSnakeLength(); i++)
                {
                    yield return bodyUpdateWait;
                    if (i == 0)
                    {
                        _position = GetSnakePosition();
                        if (bodySpawner.GetElementAtIndex(0) != null)
                            bodySpawner.GetElementAtIndex(0).SetFinalPosition(_position);
                    }
                    else if (bodySpawner.GetElementAtIndex(i - 1) != null)
                    {
                        _position = bodySpawner.GetElementAtIndex(i - 1).transform.position;
                        _position.y -= snakeBodyGap;
                        bodySpawner.GetElementAtIndex(i).SetFinalPosition(_position);
                    }
                    else
                        Debug.Log("Invalid index");

                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
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
        else if (col.transform.CompareTag("SnakeBody"))
        {
        }
    }
}
