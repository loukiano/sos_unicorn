using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Kickable : MonoBehaviour
{
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public Transform t;
    public Dashable dash;

    //kick variables
    public float kickCooldown;
    public float kickDuration;
    public float kickVel;
    public float kickDmgScale;
    public float kickSize;
    public bool isKicking;
    private bool canKick;
    private Vector3 normalTransformScale;

    // Use this for initialization
    void Start()
	{
        canKick = true;
        kickCooldown = 1;
        kickVel = 10;
        kickDuration = 0.3f;
        kickSize = 1.5f;
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        dash = GetComponent<Dashable>();
        t = GetComponent<Transform>();
        normalTransformScale = t.localScale;
    }

    public void DoKick()
    {
        if (canKick)
        {
            StartCoroutine(Kick());
            gameObject.SendMessage("StartKick"); // allows for action on dash start
        }
    }

    public bool CanKick()
    {
        return canKick;
    }

    private IEnumerator Kick()
    {

        canKick = false;
        isKicking = true;

        float tempKDS = kickDmgScale;
        //box.size *= kickSize;
        float velocityMagnitude = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);
        if (velocityMagnitude < 40.0f)
        {
            t.localScale = normalTransformScale * kickSize;
        }
        else if (velocityMagnitude < 80.0f)
        {
            t.localScale = normalTransformScale * kickSize * 2;
            kickDmgScale *= 2;
        }
        else
        {
            t.localScale = normalTransformScale * kickSize * 3;
            kickDmgScale *= 3;
        }

        Vector2 addVel = new Vector2 (0, 1);
        addVel *= kickVel;
        
        if (dash.isDashing || velocityMagnitude > 40.0f)
            rb.velocity += addVel;
        else
        {
            rb.velocity = addVel;
        }

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
}

