using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public GameObject p;
    public GameObject deathMessage;
    public GameObject deathScore;

    // Start is called before the first frame update
    void Start()
    {
        deathMessage.SetActive(false);
        deathScore.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (p.GetComponent<Player>().isDead)
        {
            deathMessage.SetActive(true);
            deathScore.SetActive(true);
        }
        
        
    }
}
