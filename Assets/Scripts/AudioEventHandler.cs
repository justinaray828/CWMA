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
        EventManager.StartListening("ButtonClicked", ButtonClicked);

        string sceneName = SceneManager.GetActiveScene().name;

        //If no scene music is playing, start the scene music
        if (!am.IsMusicPlaying()) { am.PlayMusic(sceneName); }
        else { am.SceneTransition(sceneName); }

        switch (sceneName)
        {
            case "01a_StartUp":
                Setup_StartUp();
                break;
            case "02_1stCar":
                Setup_1stCar();
                break;
            case "03_Theater":
                Setup_Theater();
                break;
            case "04_2ndCar":
                Setup_2ndCar();
                break;
            case "05_Resturant":
                Setup_Resturant();
                break;
            case "06_Ending_SecondDate":
                Setup_SecondDate();
                break;
            case "07_Ending_Jordy":
                Setup_Jordy();
                break;
            case "08_Ending_Alone":
                Setup_Alone();
                break;
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
        EventManager.StopListening("ButtonClicked", ButtonClicked);
    }

    private void Setup_StartUp()
    {
        am.LoadMusic("02_1stCar");
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
        am.LoadMusic("03_Theater");
    }

    private void Setup_Theater()
    {
        am.FadeInSFX("theater_lobby_loop");
        EventManager.StartListening("EnterBrainRoomQuick",EnterBrainRoomQuick);
        EventManager.StartListening("ExitBrainRoom", ExitBrainRoom);
        EventManager.StartListening("StartMovie", StartMovie);
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

    public void ButtonHover()
    {
        am.PlayFX("woodblock_"+UnityEngine.Random.Range(1, 2), 0.1f);
    }

    private void SceneChange()
    {
        am.SceneEnd();
    }

    private void ButtonClicked()
    {
        am.PlayPop();
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

    private void StartMovie()
    {
        am.FadeOutSFX("theater_lobby_loop");
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
        am.SetSourceOutput("woodblock_1", "Headroom");
        am.SetSourceOutput("woodblock_2", "Headroom");
    }
    private void ExitBrainRoom()
    {
        am.SetBrainroomSnapshot(false, 2f);
        am.SetSourceOutput("blip", "SFX");
        am.SetSourceOutput("woodblock_1", "SFX");
        am.SetSourceOutput("woodblock_2", "SFX");
    }
    private void EnterBrainRoomQuick()
    {
        am.SetBrainroomSnapshot(true, 0f);
        am.SetSourceOutput("blip", "Headroom");
        am.SetSourceOutput("woodblock_1", "Headroom");
        am.SetSourceOutput("woodblock_2", "Headroom");
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
