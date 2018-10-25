using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    void Start()
    {

    }

    public void DeactivateBG(float time)
    {
        StartCoroutine(Co_DeactivateBG(time));
    }

    private IEnumerator Co_DeactivateBG(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Head"))
        {
            FindObjectOfType<BGSpawner>().SpawnBG();
            DeactivateBG(3f);
        }

    }
}
