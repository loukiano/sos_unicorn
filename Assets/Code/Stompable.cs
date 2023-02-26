using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompable : MonoBehaviour
{
    [SerializeField]
    private float dropForce = 5f;

    [SerializeField]
    private float stopTime = 0.5f;

    [SerializeField]
    private float gravityScale;

    private Player p;
    public KeyCode stompKey = KeyCode.L;
    private bool doStomp = false;
    private Rigidbody2D rb;
    public bool isStomping = false;

    public Transform t;
    public BoxCollider2D box;
    
    //kick variables i'm stealing lol
    public float kickCooldown;
    public float kickDuration;
    public float kickVel;
    public float kickDmgScale;
    public float kickSize;
    public bool isKicking;
    private bool canKick;
    private Vector3 normalTransformScale;

    private void Start()
    {
        canKick = true;
        kickCooldown = 1;
        kickVel = 10;
        kickDuration = 0.3f;
        kickSize = 1.5f;
        box = GetComponent<BoxCollider2D>();
        t = GetComponent<Transform>();
        normalTransformScale = t.localScale;
    }

    private void Awake()
    {
        p = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(stompKey))
        {
            if (!p.IsGrounded())
            {
                doStomp = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (doStomp)
        {
            Stomp();
        }
        doStomp = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5)
        {
            if (isStomping)
            {
                StartCoroutine(Explode());
            }
            CompleteStomp();
        }
        
    }

    private void Stomp()
    {
        Debug.Log("NYOOM!");
        isStomping = true;
        StopAndSpin();
        StartCoroutine( DropAndSmash());
    }

    private void StopAndSpin()
    {
        ClearForces();
        gravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    private IEnumerator DropAndSmash()
    {
        yield return new WaitForSeconds(stopTime);
        rb.AddForce(Vector2.down * dropForce, ForceMode2D.Impulse);


    }

    private IEnumerator Explode()
    {
        float tempKDS = kickDmgScale;
        //box.size *= kickSize;
        float velocityMagnitude = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);

        Vector2 addVel = new Vector2(0, 1);
        addVel *= kickVel;

        if (velocityMagnitude < 40.0f)
        {
            t.localScale = normalTransformScale * kickSize;
        }
        else if (velocityMagnitude < 80.0f)
        {
            t.localScale = normalTransformScale * kickSize * 2;
            kickDmgScale *= 2;
            rb.velocity += addVel;
        }
        else
        {
            t.localScale = normalTransformScale * kickSize * 3;
            kickDmgScale *= 3;
            rb.velocity += addVel;
        }
        /*
        else
        {
            rb.velocity = addVel;
        }*/

        //float damage = rb.velocity.magnitude * kickDmgScale;
        //Debug.Log("Damage: " + damage);

        yield return new WaitForSeconds(kickDuration);

        //box.size = boxSize;
        t.localScale = normalTransformScale;
        kickDmgScale = tempKDS;

        isKicking = false;

        yield return new WaitForSeconds(kickCooldown);
        canKick = true;
    }

    private void CompleteStomp()
    {
        rb.gravityScale = gravityScale;
        isStomping = false;
    }
    private void ClearForces()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}
