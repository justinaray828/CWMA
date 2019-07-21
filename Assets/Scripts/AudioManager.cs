using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource fxSource;
    public AudioSource musicSource;
    public AudioMixer mixer;

    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
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
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetMasterVolume(float vol)
    {
        mixer.SetFloat("MasterVolume", vol);
    }

    public void SetMusicVolume(float vol)
    {
        mixer.SetFloat("MusicVolume", vol);
    }

    public void SetFxVolume(float vol)
    {
        mixer.SetFloat("SFXVolume", vol);
    }

    private void Play(Sound s)
    {
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
