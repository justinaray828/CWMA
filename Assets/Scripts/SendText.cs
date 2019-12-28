using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendText : MonoBehaviour
{
    public GameObject typedMessage;
    public GameObject sentMessage;
    public SceneHandler sceneHandler;
    public string sceneToLoad = "02_1stCar";

    float time = 3;
    private bool startTimer = false;
    

    // Update is called once per frame
    void Update()
    {
        if(startTimer)
        {
            time -= Time.deltaTime;
            if(time <= 0)
            { 
                sceneHandler.LoadScene(sceneToLoad);
            }
        }
    }

    public void SendMessage()
    {
        typedMessage.SetActive(false);
        sentMessage.SetActive(true);
        startTimer = true;
    }
}
