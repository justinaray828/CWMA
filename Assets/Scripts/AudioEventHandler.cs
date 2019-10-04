using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventHandler : MonoBehaviour
{
    public AudioManager amPrefab;
    public AudioListener al;

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
        EventManager.StartListening("Car_go", Car_go);
        EventManager.StartListening("Car_stop", Car_stop);
        EventManager.StartListening("Pause", Pause);
        EventManager.StartListening("Unpause", Unpause);
    }

    void OnDisable()
    {
        EventManager.StopListening("Car_go", Car_go);
        EventManager.StopListening("Car_stop", Car_stop);
        EventManager.StopListening("Pause", Pause);
        EventManager.StopListening("Unpause", Unpause);
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
