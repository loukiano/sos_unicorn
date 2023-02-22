using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTransition : MonoBehaviour
{
    public GameObject background;
    public GameObject tutorialText;
    public Color pinkColor;


    public bool tutorial = true;
    private Transform backgroundTransform;
    private SpriteRenderer backgroundSpriteRenderer;
    public World world;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (background != null)
        {
            backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();
            world = background.GetComponent<World>();
        }


        playerTransform = GameObject.Find("Player").transform;
        if (!world.hasTutorial)
        {
            StartGame();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        tutorial = false;
        tutorialText.SetActive(false);
        backgroundSpriteRenderer.color = pinkColor;
    }

    public bool FinishedTutorialHuh()
    {
        return !tutorial;
    }
}
