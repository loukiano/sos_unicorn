using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public int myId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        if (obj.GetComponent<Player>() != null)
        {
            Debug.Log("Checkpoint " + myId + " passed!");
            World.CheckpointPassed(myId);
        }
    }
}
