using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SignPostText : MonoBehaviour
{

    public GameObject signTextObject;
    public string tag;

    private BoxCollider2D signCollider;
    private TMP_Text signText;
    private Transform t;

    // Start is called before the first frame update
    void Start()
    {
        signCollider = GetComponent<BoxCollider2D>();
        signText = signTextObject.GetComponent<TMP_Text>();
        t = signTextObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidingObject = collision.gameObject;

        Debug.Log("Collision!");
        if (collidingObject.GetComponent<Player>() != null)
        {
            Debug.Log("Displaying text...");
            DisplayText();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject collidingObject = collision.gameObject;
        
        if (collidingObject.GetComponent<Player>() != null)
        {
            RemoveText();
        }
    } */

    public void DisplayText()
    {
        if (tag == "title")
        {
            t.localPosition = new Vector3(0, 300f, 0f);
            signText.fontSize = 200f;
            signText.text = "SOS Unicorn";
        }
        if (tag == "dash")
            signText.text = "Jump, jump, dash to get on top of the castle!";
    
    }

    public void RemoveText()
    {
        t.localPosition = new Vector3(-200f, 300f, 0f);
        signText.fontSize = 100f;
        signText.text = "";
    }
}
