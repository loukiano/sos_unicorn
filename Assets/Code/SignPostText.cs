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

    public TMP_FontAsset titleFont;
    public TMP_FontAsset hintFont;
    public Color titleColor;
    public Color hintColor;

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
            signText.font = titleFont;
            t.localPosition = new Vector3(0, 300f, 0f);
            signText.fontSize = 200f;
            signText.color = titleColor;
            signText.text = "SOS Unicorn";
        }
        else if (tag == "dash")
            signText.text = "Jump, Jump, Dash\nto get on top of the castle!";
        else if (tag == "jump")
            signText.text = "Hmm, such a tall climb!\nTry using your Jump out of your Dash!";
        else if (tag == "danger")
        {
            signText.text = "Be careful!\nFaeries are more dangerous out here.\nTry using your Puff out of your Dash to deal with big swarms!";
        }
    
    }

    public void RemoveText()
    {
        signText.font = hintFont;
        t.localPosition = new Vector3(0f, -200f, 0f);
        signText.fontSize = 80f;
        signText.color = hintColor;
        signText.text = "";
    }
}
