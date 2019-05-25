using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public float masterVolume;
    public float fxVolume;
    public float musicVolume;

    public OptionData(OptionsController optionsController)
    {
        masterVolume = optionsController.GetMasterVolume();
        fxVolume = optionsController.GetFxVolume();
        musicVolume = optionsController.GetMusicVolume();
    }
}
