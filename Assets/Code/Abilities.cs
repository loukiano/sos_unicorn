using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public GameObject p;
    public Image jumpCover;
    public Image puffCover;
    public Image dashCover;
    public Image stompCover;

    private Dashable dash;
    private Stompable stomp;
    private Kickable kick;
    private Jumpable jump;

    // Start is called before the first frame update
    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");

        dash = p.GetComponent<Dashable>();
        stomp = p.GetComponent<Stompable>();
        kick = p.GetComponent<Kickable>();
        jump = p.GetComponent<Jumpable>();

        dashCover.GetComponent<Image>();
        puffCover.GetComponent<Image>();
        stompCover.GetComponent<Image>();
        jumpCover.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dash.CanDash())
        {
            dashCover.enabled = false;
        }
        else if (!dash.CanDash())
        {
            dashCover.enabled = true;
        }
        
        if (jump.canJump)
        {
            //Debug.Log("can jump");
            jumpCover.enabled = false;
        } 
        else if (!jump.canJump)
        {
            //Debug.Log("no jumps left");
            jumpCover.enabled = true;
        }

        if (stomp.canStomp)
        {
            stompCover.enabled = false;
        }
        else if (!stomp.canStomp)
        {
            stompCover.enabled = true;
        }

        if (p.GetComponent<Player>().canFire)
        {
            puffCover.enabled = false;
        } 
        else if (!p.GetComponent<Player>().canFire)
        {
            puffCover.enabled = true;
        }
    }
}
