using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    public AudioSource fxSource;
    public AudioMixer mixer;

    public float fadeTimeDefault;

    public Sound[] MusicSounds;
    public Sound[] SFXSounds;
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

        //intialize audiosource for opening music
        Sound s = Array.Find(MusicSounds, sound => sound.name == "Scene1");
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.playOnAwake = s.playonawake;

        s.source.outputAudioMixerGroup = s.output;

        s.originalVolume = s.volume;

        //sources were not truly playing on awake. Perhaps they were being created after awake had been called?
        //regardless, this manually takes 
        if (s.playonawake)
            s.source.Play();
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

    public void SceneTransition(int nextSceneNumber)
    {
        string nextSoundName = "Scene" + nextSceneNumber;
        string currentSoundName = "Scene" + (nextSceneNumber - 1);

        Sound nextS = Array.Find(MusicSounds, sound => sound.name == nextSoundName);
        if (nextS == null)
        {
            Debug.LogWarning("Sound: " + nextSoundName + " was not found!");
            return;
        }
        Sound currentS = Array.Find(MusicSounds, sound => sound.name == currentSoundName);

        nextS.source = gameObject.AddComponent<AudioSource>();
        nextS.source.clip = nextS.clip;
        nextS.source.volume = nextS.volume;
        nextS.source.outputAudioMixerGroup = nextS.output;
        nextS.source.pitch = nextS.pitch;
        nextS.source.loop = nextS.loop;
        nextS.source.playOnAwake = nextS.playonawake;

        StartCoroutine(FadeIn(nextS.source, fadeTimeDefault));
        StartCoroutine(FadeOut(currentS.source, fadeTimeDefault));

        return;
    }

    public void PlayPop()
    {
        int index = UnityEngine.Random.Range(1, 3);
        PlayFX("pop" +index.ToString());
    }

    public void PlayBlip()
    {
        Sound s = Array.Find(SFXSounds, sound => sound.name == "blip");
        if(s.source == null)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.output;
            s.source.pitch = s.pitch; //may alter per character?
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playonawake;
        }
        s.source.Play();
    }

    public void StopBlip()
    {
        Sound s = GetSound("blip");
        if (s.source == null)
        {
            Debug.Log("cannot stop blip, blip source has not been initialzied");
        }
        s.source.Stop();
    }

    [YarnCommand("PlayFX")]
    public void PlayFX(string s)
    {
        Sound currentS = GetSound(s);
        if(currentS != null) { fxSource.PlayOneShot(currentS.clip, currentS.volume); }
    }

    [YarnCommand("Play")]
    public void Play(string s)
    {
        Sound currentS = GetSound(s);
        if(currentS.source == null) { MakeSource(currentS); }
        currentS.source.Play();
    }

    public void FadeInSFX(string name)
    {
        Sound s = GetSound(name);
        if (s.source == null)
        {
            MakeSource(s);
        }
        StartCoroutine(FadeIn(s.source, fadeTimeDefault));
    }

    public void FadeOutSFX(string name)
    {

        Sound s = GetSound(name);
        if (s.source == null)
        {
            return;
        }
        StartCoroutine(FadeOut(s.source, fadeTimeDefault));
    }

    private void MakeSource(Sound s)
    {
        if (s.source == null)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.output;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playonawake;
        }
        return;
    }

    private Sound GetSound(string str)
    {
        Sound sound = Array.Find(SFXSounds, s => s.name == str);
        if (sound == null)
        {
            Debug.LogWarning("Sound name not found in FX array: " + str);
            return null;
        }
        return sound;
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
