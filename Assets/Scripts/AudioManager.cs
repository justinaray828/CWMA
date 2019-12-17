using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    public AudioSource fxSource;
    public AudioMixer mixer;
    public AudioMixerSnapshot def;
    public AudioMixerSnapshot inBrainRoom;

    public float fadeTimeDefault;

    public Sound[] MusicSounds;
    public Sound[] SFXSounds;
    public static AudioManager instance;

    public string currSceneName;

    enum SoundType { FX, Music, Both };

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

        //since this object is just waking up, lets instantiate the variable.
        //Event Manager will set scene name.
        currSceneName = null;
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

    public void SetBrainroomSnapshot(bool state, float transitionTime = 1f)
    {
        if(state == false)
        {
            def.TransitionTo(transitionTime);
        }
        else
        {
            inBrainRoom.TransitionTo(transitionTime);
        }
    }

    public void SetSourceOutput(string soundName, string groupName)
    {
        AudioMixerGroup[] aga = mixer.FindMatchingGroups(groupName);
        if (aga != null) {
            AudioMixerGroup ag = Array.Find(aga, group => group.name == groupName);
            if(ag != null)
            {
                GetSound(soundName).source.outputAudioMixerGroup = ag;
                return;
            }
        }
        Debug.Log("could not find AudioMixerGroup name: " + groupName);

        return;
    }

    public void SceneTransition(string nextSceneName)
    {
        Sound nextS = GetSound(nextSceneName, SoundType.Music);
        Sound currentS = GetSound(currSceneName, SoundType.Music);

        MakeSource(nextS);

        StartCoroutine(FadeIn(nextS.source, fadeTimeDefault));
        StartCoroutine(FadeOut(currentS.source, fadeTimeDefault));

        currSceneName = nextSceneName;

        return;
    }

    public void SceneEnd()
    {
        foreach (Sound s in SFXSounds)
        {
            if (s.source != null)
            {
                if (s.source.isPlaying)
                {
                    StartCoroutine(FadeOut(s.source, fadeTimeDefault));
                }
            }
        }
        return;
    }

    public void PlayPop()
    {
        int index = UnityEngine.Random.Range(1, 3);
        PlayFX("pop" +index.ToString());
    }

    public void PlayBlip(float pitch)
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
        s.source.pitch = pitch;
        s.source.time = UnityEngine.Random.Range(0, s.source.clip.length);
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
        Sound currentS = GetSound(s, SoundType.FX);
        if(currentS != null) { fxSource.PlayOneShot(currentS.clip, currentS.volume); }
    }

    [YarnCommand("Play")]
    public void Play(string s)
    {
        Sound currentS = GetSound(s);
        if(currentS.source == null) { MakeSource(currentS); }
        currentS.source.Play();
    }

    public void PlayMusic(string s)
    {
        Sound currentS = GetSound(s, SoundType.Music);
        if (currentS.source == null) { MakeSource(currentS); }
        currentS.source.Play();
    }

    public void Stop(string name)
    {
        Sound sound = GetSound(name);
        if(sound.source == null) { return; }
        sound.source.Stop();
    }

    public void FadeInSFX(string name)
    {
        Sound s = GetSound(name);
        if (s.source == null)
        {
            MakeSource(s);
        }
        StartCoroutine(FadeIn(s.source, fadeTimeDefault, s.volume));
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
            s.source.ignoreListenerPause = s.ignoreListenerPause;
        }
        return;
    }

    private Sound GetSound(string str, SoundType type = SoundType.Both)
    {
        Sound sound = null;
        if (type == SoundType.FX || type == SoundType.Both)
        {
            sound = Array.Find(SFXSounds, s => s.name == str);
        }
        if (sound == null && (type == SoundType.Music || type == SoundType.Both))
        {
            sound = Array.Find(MusicSounds, s => s.name == str);
        }
        if (sound == null)
        {
            Debug.LogWarning("Sound name not found in "+type.ToString()+" array: " + str);
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

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float maxVol = 1f)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < maxVol)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

}
