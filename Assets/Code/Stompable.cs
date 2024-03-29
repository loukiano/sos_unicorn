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
    public bool canStomp;
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
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5)
        {   
            if (isStomping)
            {
                CompleteStomp();
            }
        }
        
    }

    public void DoStomp()
    {
        if (!p.IsGrounded() && canStomp)
        {
            Stomp();
        }
    }

    private void Stomp()
    {
        
        Debug.Log("NYOOM!");
        canStomp = false;
        isStomping = true;
        StopAndSpin();
        StartCoroutine(DropAndSmash());
        
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
        if (isStomping)
            // if we're still stomping
        {
            rb.AddForce(Vector2.down * dropForce, ForceMode2D.Impulse);
            SoundPlayer.PlaySound(SoundPlayer.Sounds.stompFall);
        }

    }

    private IEnumerator Explode()
    {
        float tempKDS = stompDmgScale;
        //box.size *= kickSize;
        float velocityMagnitude = Mathf.Sqrt(rb.velocity.y * rb.velocity.y);

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
        StartCoroutine(Explode());
        rb.gravityScale = gravityScale;
        isStomping = false;
        canStomp = true;
        SoundPlayer.StopSound(SoundPlayer.Sounds.stompFall);
    }
    private void ClearForces()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }


    // Make all other actions cancel the stomp
    public void DoJump()
    {
        if (isStomping)
        {
            CompleteStomp();
        }
    }

    public void DoDash()
    {
        if (isStomping)
        {
            CompleteStomp();
        }
    }

    public void DoFire()
    {
        if (isStomping)
        {
            CompleteStomp();
        }
    }
}
