using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource fightMusic;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    public void PauseBackgroundMusic()
    {
        backgroundMusic.Pause();
    }

    public void PlayFightMusic()
    {
        fightMusic.Play();
    }
}
