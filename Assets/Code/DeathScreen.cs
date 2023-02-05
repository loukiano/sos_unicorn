using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public GameObject p;
    public GameObject deathMessage;
    public GameObject deathScore;
    private Player playerComponent;

    // Start is called before the first frame update
    void Start()
    {
        deathMessage.SetActive(false);
        deathScore.SetActive(false);
        playerComponent = p.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerComponent)
        {
            Debug.Log("player component is null");
        }
        if (playerComponent.isDead)
        {
            deathMessage.SetActive(true);
            deathScore.SetActive(true);
        }
    }
}
