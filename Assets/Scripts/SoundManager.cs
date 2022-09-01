using System;
using UnityEngine;

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource soundEffects;
    public AudioSource soundMusic;

    public SoundType[] soundArray;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(Sounds sound)
    {
        AudioClip clip = FindSoundClip(sound);

        if(clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }
    }

    public void PlayEffects(Sounds sound)
    {
        AudioClip clip = FindSoundClip(sound);

        if(clip != null)
        {
            soundEffects.PlayOneShot(clip);
        }
    }

    public void StopMusic()
    {
        soundMusic.Stop();
    }
    public void StopPlayEffect()
    {
        soundEffects.Stop();
    }

    private AudioClip FindSoundClip(Sounds snd)
    {
        for(int i = 0; i < soundArray.Length; i++)
        {
            if (soundArray[i].soundType == snd)
            {
                return soundArray[i].soundClip;
            }
        }
        return null;
    }
}
