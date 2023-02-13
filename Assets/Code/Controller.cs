using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Controller : MonoBehaviour
{
    static public float deadzone = 0.3f;

    public KeyCode upButton = KeyCode.W;
    public KeyCode downButton = KeyCode.S;
    public KeyCode leftButton = KeyCode.A;
    public KeyCode rightButton = KeyCode.D;

    public KeyCode jumpKey = KeyCode.Space;

    public KeyCode dashKey = KeyCode.J;

    public KeyCode kickKey = KeyCode.K;

    Player p;

    private TutorialTransition tutorialTransition;

    // Dash Indicator
    private DashIndicator dashIndicator;

    // Use this for initialization
    void Start()
	{
        p = GetComponent<Player>();
        tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
        dashIndicator = GetComponent<DashIndicator>();
    }

    void Update()
    {

        if (!tutorialTransition.FinishedTutorialHuh())
        {
            MaybeDash();
        }
        else
        {
            MaybeJump();
            MaybeStopJump();
            MaybeDash();
            //MaybeKick();
        }
        
 
    }

    void FixedUpdate()
    {
        
    }

    static public float GetAxisFilter(string axis)
    {
        float input = Input.GetAxis(axis);

        if (Mathf.Abs(input) < deadzone)
        {
            return 0;
        }
        return input;
    }


    public Vector2 GetInputDir()
    {
        float xin = 0.0f;
        float yin = 0.0f;

        if (tutorialTransition.FinishedTutorialHuh())
        {
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
                xin = Mathf.Abs(xin) < deadzone ? 0 : xin; // deadzone filter

                yin = -Input.GetAxis("Vertical");
                yin = Mathf.Abs(yin) < deadzone ? 0 : yin; // deadzone filter


                //float y2 = Controller.GetAxisFilter("RightVertical");
                //float x2 = Controller.GetAxisFilter("RightHorizontal");
                //Debug.Log("left: (" + xin + ", " + yin + ")\nright: (" + x2 + ", " + y2 + ")");
            }
        }

        return new Vector2(xin, yin);

    }

    private void MaybeDash()
    {
        if (Input.GetKeyDown(dashKey) || Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dash!!");
            gameObject.SendMessage("DoDash", GetInputDir());
        }
    }

    private void MaybeKick()
    {
        if (Input.GetKeyDown(kickKey) || Input.GetButtonDown("Kick"))
        {
            Debug.Log("kicking!");
            gameObject.SendMessage("DoKick");
        } 
    }

    private void MaybeJump()
    {
        if (Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Sending Jump!");
            gameObject.SendMessage("DoJump");
        }
    }

    private void MaybeStopJump()
    {
        if (Input.GetKeyUp(jumpKey) || Input.GetButtonUp("Jump"))
        {
            //Debug.Log("SendingStop");
            gameObject.SendMessage("StopJump");
        }
    }




}

