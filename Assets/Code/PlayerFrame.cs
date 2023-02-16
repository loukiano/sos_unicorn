using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrame : MonoBehaviour
{

    SpriteRenderer spr;

    public float colorAlpha;
    public Kickable kick;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        kick = player.GetComponent<Kickable>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!kick.CanKick())
        {
            spr.color = new Color(0, 0, 0, 0);
        }
        else
        {
            spr.color = new Color(player.kickColor.r, player.kickColor.g, player.kickColor.b, colorAlpha);
        }
    }
}
