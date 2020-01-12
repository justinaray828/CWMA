using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    public GameObject title;
    public GameObject team;
    public GameObject assets;
    public GameObject audio;
    public GameObject specialThanks;
    public GameObject thankYou;
    public SceneHandler sceneHandler;
    public string sceneToLoad = "01_StartUp";

    float totalTime = 3;
    float time = 3;
    int stage = 0;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            stage++;
            time = totalTime;
        }

        switch (stage)
        {
            case 1:
                title.SetActive(false);
                team.SetActive(true);
                break;
            case 2:
                team.SetActive(false);
                assets.SetActive(true);
                break;
            case 3:
                assets.SetActive(false);
                audio.SetActive(true);
                break;
            case 4:
                audio.SetActive(false);
                specialThanks.SetActive(true);
                break;
            case 5:
                specialThanks.SetActive(false);
                thankYou.SetActive(true);
                break;
            case 6:
                sceneHandler.LoadScene(sceneToLoad);
                break;
            default:
                break;
        }
    }
}
