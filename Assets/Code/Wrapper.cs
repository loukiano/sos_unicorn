using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrapper : MonoBehaviour
{
    // Die once kids die
    void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
