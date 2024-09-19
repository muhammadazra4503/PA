using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        MusicVolume(savedVolume);

        // Listen for scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;

        PlayMusic("Theme"); // Play theme on start
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // List of scenes where Theme should play
        string[] themeScenes = { "MainMenu", "OptionMenu", "CollectiblesMenu", "Credit", "StageMenu" };

        // Check if the current scene is one of the theme scenes
        if (Array.Exists(themeScenes, sceneName => sceneName == scene.name))
        {
            // If the Theme is not already playing, play it
            if (musicSource.clip == null || musicSource.clip.name != "Theme" || !musicSource.isPlaying)
            {
                PlayMusic("Theme");
            }
        }
        else
        {
            // Stop the Theme music if it's playing and entering a non-theme scene
            if (musicSource.isPlaying && musicSource.clip.name == "Theme")
            {
                StopMusic();
            }
        }
    }


    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.LogError("Music sound not found: " + name);
            return;
        }

        musicSource.clip = sound.clip;
        musicSource.loop = sound.loop;  // Ensure music loops if needed
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.LogError("SFX sound not found: " + name);
            return;
        }

        sfxSource.PlayOneShot(sound.clip);
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
}
