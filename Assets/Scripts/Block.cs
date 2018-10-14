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
    }
    #endregion


    void Start()
    {
        int.TryParse(blockText.text, out counter);
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("BodyPoint"))
        {
            Destroy(col.gameObject);
            UpdateText();
        }
    }

    private void UpdateText()
    {
        counter--;
        if (counter > 0)
            SetNumber(counter);
        else
            gameObject.SetActive(false);
    }
}
