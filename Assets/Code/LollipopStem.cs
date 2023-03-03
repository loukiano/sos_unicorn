using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LollipopStem : MonoBehaviour
{
    public GroundedAI groundedAI;
    private Transform t;

    // Start is called before the first frame update
    void Start()
    {
        // groundedAI = enemy.GetComponent<GroundedAI>();
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = FindRaycast();
        float newLocalScaleY;
        print("collider: " + hit.collider);
        if (hit.collider != null)
        {
            float distance = Mathf.Abs(hit.point.y - t.position.y);
            newLocalScaleY = distance;
            t.localScale = new Vector3(t.localScale.x, 8.5f * distance, t.localScale.z);
        } else {
            newLocalScaleY = 0;
        }
    }

    public RaycastHit2D FindRaycast()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        RaycastHit2D lookDown = Physics2D.Raycast(t.position, Vector2.down, groundedAI.maxFallDist, groundMask);
        return lookDown;
    }
}
