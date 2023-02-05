using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DeathScreen : MonoBehaviour
{
    public GameObject p;
    public GameObject deathMessage;
    public GameObject deathScore;
    public GameObject scoreValue;
    public GameObject scoreText;
    private Player playerComponent;
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        deathMessage.SetActive(false);
        deathScore.SetActive(false);
        playerComponent = p.GetComponent<Player>();
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerComponent)
        {
            Debug.Log("player component is null");
        }
        if (playerComponent.isDead && !gameOver)
        {
            gameOver = true;
            TMP_Text deathScoreText = deathScore.GetComponent<TMP_Text>();
            TMP_Text scoreValueText = scoreValue.GetComponent<TMP_Text>();
            deathScoreText.text += " " + scoreValueText.text;
            scoreText.SetActive(false);
            scoreValue.SetActive(false);
            deathMessage.SetActive(true);
            deathScore.SetActive(true);
        }
    }
}
