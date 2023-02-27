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

    private float RotateSpeed = 3.0f;
    private Player p;
    public KeyCode stompKey = KeyCode.L;
    private bool doStomp = false;
    private Rigidbody2D rb;
    public bool isStomping = false;
    public bool isSpinning = false;

    public Transform t;
    public BoxCollider2D box;
    
    //kick variables i'm stealing lol
    public float stompCooldown;
    public float stompDuration;
    public float stompVel;
    public float stompDmgScale = 1f;
    public float stompSize;
    private bool canStomp;
    private Vector3 normalTransformScale;

    private void Start()
    {
        canStomp = true;
        stompCooldown = 1;
        stompVel = 10;
        stompDuration = 0.3f;
        stompSize = 1.5f;
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
        if (Input.GetKeyDown(stompKey) || Input.GetButtonDown("Kick"))
        {
            if (!p.IsGrounded() && canStomp)
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
        canStomp = false;
        isStomping = true;
        StopAndSpin();
        StartCoroutine( DropAndSmash());
    }

    private void StopAndSpin()
    {
        ClearForces();
        transform.Rotate(-Vector3.up * RotateSpeed * Time.deltaTime);
        gravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        isSpinning = true;
    }

    private IEnumerator DropAndSmash()
    {
        yield return new WaitForSeconds(stopTime);
        isSpinning = false;
        rb.AddForce(Vector2.down * dropForce, ForceMode2D.Impulse);


    }

    private IEnumerator Explode()
    {
        float tempKDS = stompDmgScale;
        //box.size *= kickSize;
        float velocityMagnitude = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);

        Vector2 addVel = new Vector2(0, 1);
        addVel *= stompVel;

        if (velocityMagnitude < 40.0f)
        {
            t.localScale = normalTransformScale * stompSize;
        }
        else if (velocityMagnitude < 80.0f)
        {
            t.localScale = normalTransformScale * stompSize * 2;
            stompDmgScale *= 2;
            rb.velocity += addVel;
        }
        else
        {
            t.localScale = normalTransformScale * stompSize * 3;
            stompDmgScale *= 3;
            rb.velocity += addVel;
        }
        /*
        else
        {
            rb.velocity = addVel;
        }*/

        //float damage = rb.velocity.magnitude * kickDmgScale;
        //Debug.Log("Damage: " + damage);

        yield return new WaitForSeconds(stompDuration);

        //box.size = boxSize;
        t.localScale = normalTransformScale;
        stompDmgScale = tempKDS;

        yield return new WaitForSeconds(stompCooldown);
        canStomp = true;
    }

    private void CompleteStomp()
    {
        rb.gravityScale = gravityScale;
        isStomping = false;
        canStomp = true;
    }
    private void ClearForces()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}
