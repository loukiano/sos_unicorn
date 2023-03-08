using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public static float timer;
    public static int lvlNum;
    public static int numLvls = 4;
    private float lastTimer;
    public static bool isRunning;
    public bool copyisRunning;

    public bool hasTutorial;
    private TutorialTransition tutorialTransition;

    public ScoreUI scoreUI;
    // Start is called before the first frame update

    void Start()
    {
        timer = 0;

        if (hasTutorial)
        {
            tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
        }

        if (!isRunning && !hasTutorial)
        {
            World.StartRunning();
        }
    }

    public static void CheckpointPassed(int checkpointId)
    {
        if (lvlNum % numLvls == checkpointId)
            // if we're progressing, move to next level!
        {
            lvlNum += 1;
        }
    }

    public static void StartRunning()
    {
        Debug.Log("WORLD. STARTRUNNING CALLED");
        isRunning = true;
        GameObject tutObj = GameObject.Find("Tutorial Transition");
        if (tutObj != null)
        {
            TutorialTransition tutTrans = tutObj.GetComponent<TutorialTransition>();
            if (!tutTrans.Equals(null))
            {
                //Debug.Log(tutTrans.ToString());
                tutTrans.StartGame();
            }

        }
        SoundPlayer.PlaySound(SoundPlayer.Sounds.background);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().StartBleeding();
    }

    public void AreaCleared()
    {
        scoreUI.ClearArea();
    }

    public static void PauseGame()
    {
        isRunning = false;
        SoundPlayer.PauseSound(SoundPlayer.Sounds.background);
    }

    public static void ContinueGame()
    {
        isRunning = true;
        SoundPlayer.UnPauseSound(SoundPlayer.Sounds.background);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning && hasTutorial && tutorialTransition.FinishedTutorialHuh())
        {
            isRunning = true;
        }

        if (isRunning)
        {
            timer += Time.deltaTime;
            var newTimer = Mathf.Floor(timer);
            if (newTimer > lastTimer)
            {
                //Debug.Log("WORLD TIME: " + newTimer);
            }
            lastTimer = newTimer;
        }

        copyisRunning = isRunning;
    }
}
