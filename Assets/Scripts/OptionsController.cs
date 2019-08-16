using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider fxVolumeSlider;
    public Slider musicVolumeSlider;
    private AudioManager audioManager;

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
        masterVolumeSlider.minValue = .001f;
        masterVolumeSlider.maxValue = 1f;
        fxVolumeSlider.minValue = .001f;
        fxVolumeSlider.maxValue = 1f;
        musicVolumeSlider.minValue = .001f;
        musicVolumeSlider.maxValue = 1f;

        OptionData data = SaveSystem.LoadOptionData();

        if (data != null)
        {
            masterVolumeSlider.value = data.masterVolume;
            //must also set the stored value, otherwise nothings is saved to the datefile if the 
            //sliders are not touched.
            masterVolumeValue = data.masterVolume;

            fxVolumeSlider.value = data.fxVolume;
            fxVolumeValue = data.fxVolume;

            musicVolumeSlider.value = data.musicVolume;
            musicVolumeValue = data.musicVolume;
        }
        else
        {
            masterVolumeSlider.value = 0f;
            fxVolumeSlider.value = 1f;
            musicVolumeSlider.value = 1f;
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

            audioManager.SetMasterVolume(Mathf.Log(masterVolumeValue) * 20);
            audioManager.SetFxVolume(Mathf.Log(fxVolumeValue) * 20);
            audioManager.SetMusicVolume(Mathf.Log(musicVolumeValue) * 20);
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