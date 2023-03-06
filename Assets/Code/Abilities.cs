using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{

    public Dashable dash;
    public Jumpable jump;
    public Stompable stomp;
    public Player p;

    // Start is called before the first frame update
    void Start()
    {
        dash = GetComponent<Dashable>();
        jump = GetComponent<Jumpable>();
        stomp = GetComponent<Stompable>();
        p = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
