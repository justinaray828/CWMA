using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour {

    public Slider masterVolumeSlider;
    public Slider fxVolumeSlider;
    public Slider musicVolumeSlider;
    private AudioManager audioManager;

    public float masterVolumeValue;
    public float fxVolumeValue;
    public float musicVolumeValue;

    // Use this for initialization
    void Start()
    {
        FindObjects();
        InitilizeValues();
    }

    void FindObjects()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    void InitilizeValues()
    {
        OptionData data = SaveSystem.LoadOptionData();
        if (data != null)
        {
            masterVolumeSlider.value = data.masterVolume;
            fxVolumeSlider.value = data.fxVolume;
            musicVolumeSlider.value = data.musicVolume;
        }
        else
        {
            masterVolumeSlider.value = 1;
            fxVolumeSlider.value = 1;
            musicVolumeSlider.value = 1;
        }

    }

    void OnGUI()
    {
        if (audioManager && GUI.changed)
        {
            masterVolumeValue = masterVolumeSlider.value;
            fxVolumeValue = fxVolumeSlider.value;
            musicVolumeValue = musicVolumeSlider.value;
        }
    }

    public float GetMasterVolume()
    {
        return masterVolumeValue;
    }

    public float GetFxVolume()
    {
        return fxVolumeValue;
    }

    public float GetMusicVolume()
    {
        return musicVolumeValue;
    }

    public void SaveSettings()
    {
        SaveSystem.SaveOptionsData(this);
    }
    
}


