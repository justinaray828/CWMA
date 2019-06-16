using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider fxVolumeSlider;
    public Slider musicVolumeSlider;
    private AudioManager audioManager;

    [Range(-80f, 0f)]
    public float masterVolumeValue;

    public float fxVolumeValue;
    public float musicVolumeValue;

    // Use this for initialization
    private void Start()
    {
        FindObjects();
        InitilizeValues();
    }

    private void FindObjects()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    private void InitilizeValues()
    {
        masterVolumeSlider.minValue = -80f;
        masterVolumeSlider.maxValue = 0f;

        OptionData data = SaveSystem.LoadOptionData();
        if (data != null)
        {
            Debug.Log("setting saved data values");
            masterVolumeSlider.value = data.masterVolume;
            fxVolumeSlider.value = data.fxVolume;
            musicVolumeSlider.value = data.musicVolume;
        }
        else
        {
            Debug.Log("setting default option values");
            masterVolumeSlider.value = 0f;
            fxVolumeSlider.value = 1;
            musicVolumeSlider.value = 1;
        }

        masterVolumeSlider.onValueChanged.AddListener(delegate { UpdateValues(); });
        fxVolumeSlider.onValueChanged.AddListener(delegate { UpdateValues(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { UpdateValues(); });
    }

    public void UpdateValues()
    {
        if (audioManager)
        {
            masterVolumeValue = masterVolumeSlider.value;
            fxVolumeValue = fxVolumeSlider.value;
            musicVolumeValue = musicVolumeSlider.value;

            audioManager.SetMasterVolume(masterVolumeValue);
            audioManager.SetFxVolume(fxVolumeValue);
            audioManager.SetMusicVolume(musicVolumeValue);
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