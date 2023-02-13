using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Kickable : MonoBehaviour
{
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public Dashable dash;

    //kick variables
    public float kickCooldown;
    public float kickDuration;
    public float kickVel;
    public float kickDmgScale;
    public Vector2 kickSize;
    public bool isKicking;
    private bool canKick;

    // Use this for initialization
    void Start()
	{
        //canKick = true;
        //kickCooldown = 2;
        //kickVel = 7;
        //kickDuration = 0.1f;
        //kickSize = new Vector2(1.5f, 3);
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        dash = GetComponent<Dashable>();
    }

    public void DoKick()
    {
        if (canKick)
        {
            StartCoroutine(Kick());

        }
    }

    private IEnumerator Kick()
    {

        canKick = false;
        isKicking = true;

        Vector2 boxSize = box.size;
        box.size = kickSize;

        if (dash.isDashing)
        {
            Debug.Log("adding velocity!");

            Vector2 addVel = rb.velocity;
            addVel.Normalize();
            addVel *= kickVel;
            rb.velocity += addVel;
        }

        yield return new WaitForSeconds(kickDuration);

        box.size = boxSize;

        isKicking = false;

        yield return new WaitForSeconds(kickCooldown);
        canKick = true;
    }
}

