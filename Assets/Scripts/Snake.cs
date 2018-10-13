using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float turnSpeed = 10f;
    public float moveSpeed = 10f;

    [Header("Body Stuff")]
    [SerializeField] private float snakeBodyGap;

    private SnakeBodySpawner bodySpawner;
    private BGSpawner bGSpawner;
    private Vector3 position;

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
    }
}
