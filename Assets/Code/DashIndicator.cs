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

        readyColor = new Color(165/255f, 250/255f, 198/255f);
    }

    // Update is called once per frame
    void Update()
    {
        if (dash.CanDash())
        {
            spr.color = readyColor;
        } else
        {
            spr.color = Color.gray;
        }

        Vector2 inputDir = c.GetInputDir();
        if (inputDir.magnitude == 0)
        // neutral dash
        {
            indicatorPosition = t.position;
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
            Vector2 indicatorOffset = new Vector2(newXVel * 4.8f / vectorLength, newYVel * 4.8f / vectorLength);
            float rotationAngle = 360f - Mathf.Atan(indicatorOffset.x / indicatorOffset.y) * 180f / 2f / Mathf.PI;
            indicatorPosition = new Vector2(t.position.x + indicatorOffset.x, t.position.y + indicatorOffset.y);

            //indicatorTransform.Rotate(0f, 0f, rotationAngle);
        }
        indicatorTransform.position = new Vector3(indicatorPosition.x, indicatorPosition.y, t.position.z);
    }


}
