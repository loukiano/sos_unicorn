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
    public GameObject retryButton;
    private GameObject player;
    private Health playerHealth;
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        deathMessage.SetActive(false);
        deathScore.SetActive(false);
        retryButton.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            Debug.Log("no Object with name \"Player\"");
        }
        if (playerHealth.isDead() && !gameOver)
        {
            gameOver = true;
            TMP_Text deathScoreText = deathScore.GetComponent<TMP_Text>();
            TMP_Text scoreValueText = scoreValue.GetComponent<TMP_Text>();
            deathScoreText.text += " " + scoreValueText.text;
            scoreText.SetActive(false);
            scoreValue.SetActive(false);
            deathMessage.SetActive(true);
            deathScore.SetActive(true);
            retryButton.SetActive(true);
        }
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}
