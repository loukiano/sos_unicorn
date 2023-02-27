using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteHandler : MonoBehaviour
{

    public Sprite stationary;
    public Sprite gallop;
    public Sprite dash;
    public SpriteRenderer spriteHolder;
    public string spriteTag;

    private Rigidbody2D rb;
    private Player player;
    private Dashable dashable;
    private Kickable kickable;
    private Transform transform;
    public bool facingRight;
    private bool isMoving;
    public float nextGallopTime;
    public float gallopInterval;
    public float nextStationaryTime;
    public float stationaryInterval;

    // Start is called before the first frame update
    void Start()
    {
        spriteHolder = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        dashable = GetComponent<Dashable>();
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        kickable = GetComponent<Kickable>();

        isMoving = false;
        spriteTag = "stationary";
        nextGallopTime = 0.5f;
        gallopInterval = 0.5f;
        nextStationaryTime = 0.0f;
        stationaryInterval = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashable.isDashing || kickable.isKicking)
        {
            spriteHolder.sprite = dash;
            spriteTag = "attacking";
        }
        else if (Time.time > nextGallopTime)
        {
            //Debug.Log("Time.time of GALLOP: " + Time.time);
            nextGallopTime = Time.time + gallopInterval;
            nextStationaryTime = Time.time + stationaryInterval;
            spriteHolder.sprite = gallop;
            spriteTag = "gallop";
        }
        else if (Time.time > nextStationaryTime)
        {
            //Debug.Log("Time.time of STATIONARY: " + Time.time);
            spriteHolder.sprite = stationary;
            spriteTag = "stationary";
        }

        UpdateDirection();
    }

    public void UpdateDirection()
    {
        if ((transform.localScale.x > 0 && rb.velocity.x < -0.05) || (transform.localScale.x < 0 && rb.velocity.x > 0.05))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}
