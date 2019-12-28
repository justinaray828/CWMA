using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonPress : MonoBehaviour
{
    SceneHandler sceneHandler;
    string carScene = "02_1stCar";
    string optionsScene = "01b_Options";
    string menuScene = "01a_StartUp"; //TODO: Need to implement

    public enum clickActions { Start, Options, Exit};
    public clickActions clickAction;

    private void Start()
    {
        sceneHandler = FindObjectOfType<SceneHandler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (clickAction)
        {
            case clickActions.Start:
                sceneHandler.LoadScene(carScene);
                break;
            case clickActions.Options:
                sceneHandler.LoadScene(optionsScene);
                break;
            case clickActions.Exit:
                sceneHandler.ExitGame();
                break;
            default:
                Debug.Log("Incorrect Action");
                break;
        }

    }
}
