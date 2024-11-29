using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Public audio clips
    public AudioClip backgroundMusic;
    public AudioClip[] soundEffects;

    // Audio Sources
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance of AudioManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicate AudioManager instances
            return;
        }

        // Keep the AudioManager across scenes
        DontDestroyOnLoad(gameObject);

        // Create AudioSource components for music and SFX
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Set up music audio source
        musicSource.loop = true;
        musicSource.volume = 0.5f;

        // Set up SFX audio source
        sfxSource.loop = false;
        sfxSource.volume = 1f;

        // Start background music
        PlayBackgroundMusic();
    }

    // Play background music
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && musicSource.clip != backgroundMusic)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    // Play a sound effect by passing its name
    public void PlaySoundEffect(string soundName)
    {
        AudioClip clip = GetSoundEffectByName(soundName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Get a sound effect by its name
    private AudioClip GetSoundEffectByName(string name)
    {
        foreach (var clip in soundEffects)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }
}
