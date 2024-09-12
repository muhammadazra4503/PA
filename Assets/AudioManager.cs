using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds;
    public AudioSource musicSource;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f); // Default volume is 1 (max)
        MusicVolume(savedVolume);

        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        Sound sounds = Array.Find(musicSounds, x => x.name == name);

        if(sounds == null)
        {
            Debug.Log("Sounds not found");

        }

        else
        {
            musicSource.clip = sounds.clip;
            musicSource.Play();
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;

        // Save the volume when it's changed
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    // Add StopMusic method to stop the theme
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
