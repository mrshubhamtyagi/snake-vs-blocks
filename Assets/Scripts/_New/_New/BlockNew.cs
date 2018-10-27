using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNew : MonoBehaviour
{
    void Start()
    {

    }


    private void OnCollisionStay2D(Collision2D col)
    {
        col.transform.rotation = Quaternion.identity;
    }
}
