using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public TextMeshPro blockText;

    private BlockSpawner blockSpawner;
    private int counter;

    #region SETTERS AND GETTERS
    public void SetNumber(int i)
    {
        blockText.text = i.ToString();
        counter = i;
    }
    #endregion


    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("BodyPoint"))
        {
            Destroy(col.gameObject);
            UpdateTextUI();
            foreach (Transform point in col.transform.parent)
            {
                //point.GetComponent<CircleCollider2D>().isTrigger = true;
                //point.GetComponent<CircleCollider2D>().isTrigger = false;
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.transform.CompareTag("BodyPoint"))
    //    {
    //        print("Collided");
    //    }
    //}

    public void UpdateTextUI()
    {
        counter--;
        if (counter > 0)
            SetNumber(counter);
        else
            gameObject.SetActive(false);
    }
}
