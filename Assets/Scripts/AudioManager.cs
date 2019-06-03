using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource fxSource;
    public AudioSource musicSource;
    public AudioMixer mixer;

    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        //Ensure that only one instance of this class is created (we don't want a new AudioManager everytime a scene loads)
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMasterVolume(float vol)
    {
        mixer.SetFloat("MasterVolume", vol);
    }

    public void SetMusicVolume(float vol)
    {
        musicSource.volume = vol;
    }

    public void SetFxVolume(float vol)
    {
        fxSource.volume = vol;
    }

    void Play(Sound s)
    {

    }
}
