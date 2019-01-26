using System.Collections;
using System.Collections.Generic;
using AudioToolkit;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Static reference to this AudioManager
    /// </summary>
    public static AudioManager Instance;

    /// <summary>
    /// This array gathers all the possible payable Sound Components.
    /// Playabale SoundComponents have to be put as a child of this AudioManager.
    /// </summary>
    public Sound[] Sounds;

    public Sound CurrentMusic;

    public AmbientSound[] Ambients;

    public AmbientSound CurrentAmbient;

    public List<Sound> PlayingSounds;

    /// <summary>
    /// Returns a reference of the AudioListener in the scene
    /// </summary>
    public AudioListener Listener
    {
        get
        {
            if (listener == null)
                listener = FindObjectOfType<AudioListener>();

            return listener;
        }
    }

    AudioListener listener;

    /// <summary>
    /// Returns the listener position as a Vector3
    /// </summary>
    public Vector3 ListenerPosition
    {
        get
        {
            return Listener.gameObject.transform.position;
        }
    }

    void Awake()
    {
        // If no singleton already exist
        if (Instance == null)
        {
            // Assign this as the AudioManager singleton instance
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Fill in the Sound Array
            Sounds = gameObject.GetComponentsInChildren<Sound>();
            // Initialize the PlatingSound List, so it cannot be null;
            PlayingSounds = new List<Sound>();

            Debug.Log("AUDIO TOOLKIT : AudioManager succesfully initialized");
            return;
        }

        // If an AudioManager singleton already exists
        else if (Instance != null && Instance != this)
        {
            // Destroy this gameObject
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Play a Sound if it is available in the Sounds array.
    /// </summary>
    /// <param name="soundName">The name of the Sound to Play</param>
    public void Play(string soundName)
    {
        Sound _soundToPlay = GetSound(soundName);

        if (_soundToPlay == null)
        {
            return;
        }       
        _soundToPlay.Play(Vector3.zero);

        if (_soundToPlay.PlaybackMode != SoundPlaybackMode.OneShot)
            PlayingSounds.Add(_soundToPlay);
    }

    public void Play(string soundName, Vector3 position)
    {
        Sound _soundToPlay = GetSound(soundName);

        if (_soundToPlay == null)
        {
            return;
        }
        _soundToPlay.Play(position);

        if (_soundToPlay.PlaybackMode != SoundPlaybackMode.OneShot)
            PlayingSounds.Add(_soundToPlay);
    }



    public void PlayMusic(string soundName)
    {
        Sound _soundToPlay = GetSound(soundName);

        if (_soundToPlay == null)
        {
            return;
        }

        if (CurrentMusic != null)
        {
            CurrentMusic.Stop();
        }

        _soundToPlay.Play(Vector3.zero);
        PlayingSounds.Add(_soundToPlay);
        CurrentMusic = _soundToPlay;
    }

    /// <summary>
    /// Plays a uniqe AmbientSound. Replaces the old one if needed.
    /// </summary>
    /// <param name="ambientName"></param>
    public void PlayAmbientSound(string ambientName)
    {
        // Gets the AmbientSound to play
        AmbientSound _ambientToPlay = GetAmbient(ambientName);       
        // returns if null
        if (_ambientToPlay == null)
            return;
        // return if the new AmbienSound is the same as current AmbientSound
        if (_ambientToPlay == CurrentAmbient)
        {
            Debug.Log("AudioToolkit :: You're trying to play the same AmbientSound");
            return;
        }

        // Stops all the currently playing AmbientSounds
        if (CurrentAmbient != null)
        {
            foreach (Sound _sound in CurrentAmbient.Sounds)
            {
                _sound.Stop();
            }
        }

        // Play the AmbientSound child sounds
        foreach (Sound _sound in _ambientToPlay.Sounds)
        {
            _sound.Play(Vector3.zero);
        }

        // Set the CurrentAmbient to the AmbientSound that we've just played
        CurrentAmbient = _ambientToPlay;
    }

    /// <summary>
    /// Stop all the matching named Sonds that are currently playing.
    /// </summary>
    /// <param name="soundName">The name of the Sound to Stop</param>
    public void Stop(string soundName)
    {
        // Gathers all the matching name Sound that are currently Playing in the _soundToStop List.
        List<Sound> _soundsToStop = new List<Sound>();

        foreach (Sound _sound in PlayingSounds)
        {
            if (_sound.Name == soundName)
            {
                _soundsToStop.Add(_sound);
            }
        }

        // Stop all the Sounds in the _soundsToStopList
        foreach (Sound _sound in _soundsToStop)
        {
            _sound.Stop();
            // Remove the stopped sound from the playing sound list
            PlayingSounds.Remove(_sound);
        }
    }

    /// <summary>
    /// Stop all the currently playing sounds
    /// </summary>
    public void StopAll()
    {
        foreach (Sound _sound in PlayingSounds)
        {
            _sound.Stop();
        }
    }

    /// <summary>
    /// Return a Sound component reference knowing its name.
    /// </summary>
    /// <param name="soundName">The name of the Sound component</param>
    /// <returns>The correct named Sound component if the name matches. Null if does'nt matches.</returns>
    public Sound GetSound(string soundName)
    {
        foreach (Sound _sound in Sounds)
        {
            if (_sound.Name == soundName)
            {
                return _sound;
            }
        }
        Debug.LogError("AUDIO TOOLKIT : No Sound found with name : " + soundName);
        return null;
    }

    AmbientSound GetAmbient(string ambientName)
    {
        foreach (AmbientSound _ambient in Ambients)
        {
            if (_ambient.Name == ambientName)
            {
                return _ambient;
            }
        }
        Debug.LogError("AUDIO TOOLKIT : No AmbientSound found with name : " + ambientName);
        return null;
    }
}

[System.Serializable]
public class AmbientSound
{
    public string Name;

    public Sound[] Sounds;
}
