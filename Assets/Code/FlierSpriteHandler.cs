using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlierSpriteHandler : MonoBehaviour
{
    public Sprite stationaryLevelThree;
    public Sprite flapLevelThree;
    public Sprite stationaryLevelTwo;
    public Sprite flapLevelTwo;
    public Sprite stationaryLevelOne;
    public Sprite flapLevelOne;
    public Sprite stationary;
    public Sprite flap;
    public SpriteRenderer spriteHolder;
    public string spriteTag;

    private Rigidbody2D rb;
    private Enemy enemy;
    private int currentPowerLevel;
    public float nextflapTime;
    public float flapInterval;
    public float nextStationaryTime;
    public float stationaryInterval;

    // Start is called before the first frame update
    void Start()
    {
        spriteHolder = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();

        spriteTag = "stationary";
        nextflapTime = 0.5f;
        flapInterval = 0.5f;
        nextStationaryTime = 0.0f;
        stationaryInterval = 0.2f;
        currentPowerLevel = enemy.powerLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.powerLevel != currentPowerLevel)
        {
            if (enemy.powerLevel == 1)
            {
                stationary = stationaryLevelOne;
                flap = flapLevelOne;
                currentPowerLevel = 1;
            }
            else if (enemy.powerLevel == 2)
            {
                stationary = stationaryLevelTwo;
                flap = flapLevelTwo;
                currentPowerLevel = 2;
            }
            else
            {
                stationary = stationaryLevelThree;
                flap = flapLevelThree;
                currentPowerLevel = 3;
            }
        }

        if (Time.time > nextflapTime)
        {
            //Debug.Log("Time.time of flap: " + Time.time);
            nextflapTime = Time.time + flapInterval;
            nextStationaryTime = Time.time + stationaryInterval;
            spriteHolder.sprite = flap;
            spriteTag = "flap";
        }
        else if (Time.time > nextStationaryTime)
        {
            //Debug.Log("Time.time of STATIONARY: " + Time.time);
            spriteHolder.sprite = stationary;
            spriteTag = "stationary";
        }
    }
}
