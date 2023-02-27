using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public static float timer;
    private float lastTimer;
    public static bool isRunning;
    public bool copyisRunning;

    public bool hasTutorial;
    private TutorialTransition tutorialTransition;
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

    public static void StartRunning()
    {

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
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().StartBleeding();
    }

    public static void PauseGame()
    {
        isRunning = false;
    }

    public static void ContinueGame()
    {
        isRunning = true;
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
