using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioEventHandler : MonoBehaviour
{
    public AudioManager amPrefab;

    private AudioManager am;

    private void Awake()
    {
        am = FindObjectOfType<AudioManager>();
        if (am == null)
        {
            am = Instantiate(amPrefab);
            am.name = "AudioManager";
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("Pause", Pause);
        EventManager.StartListening("Unpause", Unpause);
        EventManager.StartListening("SceneChange", SceneChange);

        string sceneName = SceneManager.GetActiveScene().name;

        //If no scene music is playing, start the scene music
        if (am.currSceneName == null)
        {
            am.PlayMusic(sceneName);
            am.currSceneName = sceneName;
        }
        else
        {
            am.SceneTransition(sceneName);
        }

        if (sceneName == "02_1stCar")
        {
            Setup_1stCar();
        }
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (sceneName == "03_Theater")
        {
            Setup_Theater();
        }
        if (sceneName == "04_2ndCar")
        {
            Setup_2ndCar();
        }
        if (sceneName == "05_Resturant")
        {
            Setup_Resturant();
        }
        if (sceneName == "06_Ending_SecondDate")
        {
            Setup_SecondDate();
        }
        if (sceneName == "07_Ending_Jordy")
        {
            Setup_Jordy();
        }
        if (sceneName == "08_Ending_Alone")
        {
            Setup_Alone();
        }
    }

    void OnDisable()
    {
        EventManager.StopListening("Pause", Pause);
        EventManager.StopListening("Unpause", Unpause);
        EventManager.StopListening("Car_go", Car_go);
        EventManager.StopListening("Car_stop", Car_stop);
        EventManager.StopListening("EnterBrainRoom", EnterBrainRoom);
        EventManager.StopListening("ExitBrainRoom", ExitBrainRoom);
        EventManager.StopListening("EnterBrainRoomQuick", EnterBrainRoomQuick);
        EventManager.StopListening("brainOpen", BrainOpen);
        EventManager.StopListening("brainClose", BrainClose);
        EventManager.StopListening("GinaWalking", GinaWalking);
        EventManager.StopListening("ReaWalking", ReaWalking);
        EventManager.StopListening("ReaWalkingAway", ReaWalkingAway);
    }

    private void Setup_1stCar()
    {
        am.FadeInSFX("car_loop");
        EventManager.StartListening("Car_go", Car_go);
        EventManager.StartListening("Car_stop", Car_stop);
        EventManager.StartListening("EnterBrainRoom", EnterBrainRoom);
        EventManager.StartListening("ExitBrainRoom", ExitBrainRoom);
        EventManager.StartListening("brainOpen", BrainOpen);
        EventManager.StartListening("brainClose", BrainClose);
    }

    private void Setup_Theater()
    {
        am.FadeInSFX("theater_lobby_loop");
        EventManager.StartListening("EnterBrainRoomQuick",EnterBrainRoomQuick);
        EventManager.StartListening("ExitBrainRoom", ExitBrainRoom);
    }

    private void Setup_2ndCar()
    {
        am.FadeInSFX("car_loop");
        EventManager.StartListening("Car_go", Car_go);
        EventManager.StartListening("Car_stop", Car_stop);
        EventManager.StartListening("EnterBrainRoom", EnterBrainRoom);
        EventManager.StartListening("ExitBrainRoom", ExitBrainRoom);
        EventManager.StartListening("brainOpen", BrainOpen);
        EventManager.StartListening("brainClose", BrainClose);
    }

    private void Setup_Alone()
    {
    }

    private void Setup_Jordy()
    {
    }

    private void Setup_SecondDate()
    {
    }

    private void Setup_Resturant()
    {
        am.FadeInSFX("restaurant_loop");
        EventManager.StartListening("EnterBrainRoomQuick", EnterBrainRoomQuick);
        EventManager.StartListening("ExitBrainRoom", ExitBrainRoom);
        EventManager.StartListening("GinaWalking", GinaWalking);
        EventManager.StartListening("ReaWalking", ReaWalking);
        EventManager.StartListening("ReaWalkingAway", ReaWalkingAway);
    }

    private void SceneChange()
    {
        am.SceneEnd();
    }

    private void Car_go()
    {
        am.PlayFX("car_go");
        am.FadeInSFX("car_loop");
    }

    private void Car_stop()
    {
        am.PlayFX("car_slowdown");
        am.FadeOutSFX("car_loop");
    }

    private void BrainOpen()
    {
        am.PlayFX("brain_open");
    }
    private void BrainClose()
    {
        am.PlayFX("brain_close");
    }

    private void GinaWalking()
    {
        am.PlayFX("footsteps2");
    }
    private void ReaWalking()
    {
        am.PlayFX("footsteps1");
    }
    private void ReaWalkingAway()
    {
        am.PlayFX("footsteps1_2");
    }

    private void EnterBrainRoom()
    {
        am.SetBrainroomSnapshot(true, 2f);
        am.SetSourceOutput("blip", "Headroom");
    }
    private void ExitBrainRoom()
    {
        am.SetBrainroomSnapshot(false, 2f);
        am.SetSourceOutput("blip", "SFX");
    }
    private void EnterBrainRoomQuick()
    {
        am.SetBrainroomSnapshot(true, 0f);
        am.SetSourceOutput("blip", "Headroom");
    }

    private void Pause()
    {
        AudioListener.pause = true;
        am.Play("pause");
        am.PlayMusic("PauseMusic");
    }

    private void Unpause()
    {
        AudioListener.pause = false;
        am.Stop("PauseMusic");
        
    }


}
