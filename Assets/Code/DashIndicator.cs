using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicator : MonoBehaviour
{
    private GameObject player;
    private Controller c;
    private Player p;
    private Rigidbody2D rb;
    private Transform t;
    private SpriteRenderer spr;
    private Vector2 indicatorPosition;
    private Dashable dash;

    private Transform indicatorTransform;
    public float colorAlpha;
    public float offsetScale;

    public Vector3 indicatorScale;

    Color readyColor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        c = player.GetComponent<Controller>();
        p = player.GetComponent<Player>();
        rb = player.GetComponent<Rigidbody2D>();
        t = player.GetComponent<Transform>();
        dash = player.GetComponent<Dashable>();
        indicatorPosition = t.position;
        indicatorTransform = GetComponent<Transform>();

        spr = GetComponent<SpriteRenderer>();
        readyColor = new Color(165/255f, 250/255f, 198/255f, colorAlpha);
        transform.localScale = indicatorScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (dash.CanDash())
        {
            spr.color = readyColor;
        } else
        {
            spr.color = unreadyColor;
        }

        Vector2 inputDir = c.GetInputDir();
        if (inputDir.magnitude == 0)
        // neutral dash
        {
            indicatorPosition = t.position;
            indicatorTransform.rotation = Quaternion.Euler(0, 0, -90);

        }
        else
        {
            float newXVel = inputDir.x * dash.dashVel;
            if (Mathf.Sign(inputDir.x) == Mathf.Sign(rb.velocity.x))
            // conserve momentum if same direction
            {
                newXVel += rb.velocity.x;
            }

            float newYVel = inputDir.y * dash.dashVel;
            if (Mathf.Sign(inputDir.y) == Mathf.Sign(rb.velocity.y))
            {
                newYVel += rb.velocity.y;
            }

            float vectorLength = Mathf.Sqrt(newXVel * newXVel + newYVel * newYVel);
            Vector2 indicatorOffset = new Vector2(newXVel * offsetScale / vectorLength, newYVel * offsetScale / vectorLength);

            float rotationAngle = 360 - Mathf.Atan2(indicatorOffset.x, indicatorOffset.y) * Mathf.Rad2Deg;
            //Debug.Log("x: " + indicatorOffset.x + ", y: " + indicatorOffset.y + ", Rotationagnel: " + rotationAngle);
            Quaternion desiredRotation = Quaternion.Euler(0, 0, rotationAngle);

            indicatorPosition = new Vector2(t.position.x + indicatorOffset.x, t.position.y + indicatorOffset.y);

            indicatorTransform.rotation = desiredRotation;
        }
        indicatorTransform.position = new Vector3(indicatorPosition.x, indicatorPosition.y, t.position.z);
    }


}
