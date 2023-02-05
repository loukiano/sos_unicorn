using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreUI : MonoBehaviour
{
    public Transform scoreText;
    public Transform scoreValue;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = this.gameObject.transform.GetChild(0);
        scoreValue = this.gameObject.transform.GetChild(1);
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Score()
    {
        score += 13;
        TMP_Text scoreText = scoreValue.GetComponent<TMP_Text>();
        int leadingZeroes = 5;
        float tempScore = score;
        string finalScoreText = "";
        
        while (tempScore >= 1)
        {
            tempScore /= 10;
            leadingZeroes -= 1;
        }

        for (int i = 0; i < leadingZeroes; i++)
        {
            finalScoreText += "0";
        }
        scoreText.text = finalScoreText + score.ToString();
    }
}
