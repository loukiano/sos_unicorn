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

    public GameObject box1;
    public GameObject box2;

    // Start is called before the first frame update
    void Start()
    {
        signCollider = GetComponent<BoxCollider2D>();
        signText = signTextObject.GetComponent<TMP_Text>();
        t = signTextObject.GetComponent<Transform>();
        box1.SetActive(false);
        box2.SetActive(false);
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
        box1.SetActive(true);
        box2.SetActive(true);

        if (tag == "title")
        {
            signText.font = titleFont;
            t.localPosition = new Vector3(0, 300f, 0f);
            signText.fontSize = 200f;
            signText.color = titleColor;
            signText.text = "SOS Unicorn";
        }
        else
        {
            signText.font = hintFont;
            t.localPosition = new Vector3(0f, -200f, 0f);
            signText.fontSize = 80f;
            signText.color = hintColor;
            if (tag == "help")
                signText.text = "If you need help collecting blood for your demon lord,\ngo find a sign post!\nSincerely, a sign post <3";
            else if (tag == "basic_controls")
                signText.text = "Lollipops ahead-- a good opportunity for blood!\nAttack by dashing (R1), ground pounding (L1),\nor shooting (Square).";
            else if (tag == "climb")
                signText.text = "You can also use your dash to help you get around.";
            else if (tag == "jump")
                signText.text = "Stuck? Jump by pressing (X)!\n(You can also double jump in the air.)";
            else if (tag == "good_luck")
                signText.text = "Enemies will get harder from here on out.\nYou won't see another sign post.\nRemember the basics, and good luck!";
        }
    }

    public void RemoveText()
    {
        box1.SetActive(false);
        box2.SetActive(false);
        signText.font = hintFont;
        t.localPosition = new Vector3(0f, -200f, 0f);
        signText.fontSize = 80f;
        signText.color = hintColor;
        signText.text = "";
    }
}
