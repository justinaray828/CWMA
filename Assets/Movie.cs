using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(SpriteRenderer))]
public class Movie : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    DialoguePanelController dialoguePanelController;
    SceneHandler sceneHandler;
    public bool startMovie = false;
    float alphaValue = 0;
    float alphaRate = .75f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dialoguePanelController = FindObjectOfType<DialoguePanelController>();
        sceneHandler = FindObjectOfType<SceneHandler>();
    }

    [YarnCommand("Start")]
    public void StartMovie()
    {
        startMovie = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(startMovie)
        {
            dialoguePanelController.PopUp = false;
            alphaValue += Time.deltaTime * alphaRate;
            spriteRenderer.color = new Color(1f, 1f, 1f, alphaValue);
        }
    }
}
