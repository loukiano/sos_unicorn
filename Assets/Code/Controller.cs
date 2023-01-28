﻿using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Controller : MonoBehaviour
{
    public float controllerDeadzone = 0.05f;

    public KeyCode upButton = KeyCode.W;
    public KeyCode downButton = KeyCode.S;
    public KeyCode leftButton = KeyCode.A;
    public KeyCode rightButton = KeyCode.D;

    public KeyCode jumpKey = KeyCode.Space;


    // Use this for initialization
    void Start()
	{
        
    }

    void FixedUpdate()
    {
        MaybeJump();
    }

    public Vector2 GetInputDir()
    {
        float xin = 0.0f;
        float yin = 0.0f;

        if (Input.GetJoystickNames().Length == 0)
            // No controller, use keyboard
        {
            if (Input.GetKey(upButton))
            {
                yin += 1;
            }
            if (Input.GetKey(downButton))
            {
                yin -= 1;
            }
            if (Input.GetKey(rightButton))
            {
                xin += 1;
            }
            if (Input.GetKey(leftButton))
            {
                xin -= 1;
            }
        }
        else
            // we have a controller -- lets use it
        {
            xin = Input.GetAxis("Horizontal");
            xin = Mathf.Abs(xin) < controllerDeadzone ? 0 : xin; // deadzone filter

            yin = Input.GetAxis("Vertical");
            yin = Mathf.Abs(yin) < controllerDeadzone ? 0 : yin; // deadzone filter
        }

        return new Vector2(xin, yin);

    }

    private void MaybeJump()
    {
        if (Input.GetKey(jumpKey) || Input.GetButton("Jump"))
        {
            gameObject.SendMessage("DoJump");
        }
    }


    
}
