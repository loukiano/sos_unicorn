using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTransition : MonoBehaviour
{
    public GameObject background;
    public GameObject tutorialText;
    public Color pinkColor;

    public Vector2 TutorialPoint;
    public Vector2 startPoint;

    private bool tutorial;
    private Transform backgroundTransform;
    private SpriteRenderer backgroundSpriteRenderer;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        tutorial = true;
        backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();
        playerTransform = GameObject.Find("Player").transform;
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
