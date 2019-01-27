using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioToolkit
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundInstance : MonoBehaviour
    {
        Sound parentSound;

        public AudioSource source;

        public float refVolume;

        float fadeFactor = 0f;

        bool fadeIn;

        Coroutine FadeInCoroutine;

        Coroutine FadeOutCoroutine;

        Coroutine UpdateVolumeCoroutine;

        Coroutine CheckForDestroyCoroutine;

        public SoundInstance Initialize(Sound _parentSound, AudioClip _clip, bool _loop, float _refVolume, float _pitch)
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;

            parentSound = _parentSound;
            refVolume = _refVolume;

            source.maxDistance = parentSound.MaxDistance;
            source.outputAudioMixerGroup = parentSound.Output;
            source.clip = _clip;
            source.loop = _loop;
            source.pitch = _pitch;
            source.volume = 0f;

            return this;
        }

        public void Play()
        {
            source.Play();

            if (parentSound.RandomStartPosition)
            {
                source.timeSamples = Random.Range(0, source.clip.samples);
            }

            parentSound.PlayingInstances.Add(this);

            if (parentSound.PlaybackMode == SoundPlaybackMode.OneShot)
            {
                FadeInCoroutine = StartCoroutine(FadeIn());
            }
            else
            {
                UpdateVolumeCoroutine = StartCoroutine(UpdateVolume());
            }

            CheckForDestroyCoroutine = StartCoroutine(CheckForDestroy());
        }

        public void Stop()
        {
            if (parentSound.PlaybackMode == SoundPlaybackMode.OneShot)
            {
               FadeOutCoroutine  = StartCoroutine(FadeOut());
            }
        }

        IEnumerator FadeIn()
        {
            source.volume = 0f;

            if (parentSound.FadeInTime == 0f)
            {
                fadeFactor = 1f;
                source.volume = refVolume * fadeFactor;
                yield return null;
            }

            while (fadeFactor < 1f)
            {
                fadeFactor += Time.deltaTime * (1 / parentSound.FadeInTime);
                source.volume = refVolume * fadeFactor;

                if (fadeFactor >= 1f)
                    fadeFactor = 1f;
                yield return null;
            }
        }

        IEnumerator FadeOut()
        {
            if (FadeInCoroutine != null)
            {
                StopCoroutine(FadeInCoroutine);
            }
            while (fadeFactor > 0f)
            {
                fadeFactor -= Time.deltaTime * (1 / parentSound.FadeOutTime);
                source.volume = refVolume * fadeFactor;

                if (fadeFactor <= 0f)
                    fadeFactor = 0f;
                yield return null;
            }
            DestroySoundInstance();
        }

        IEnumerator CheckForDestroy()
        {
            while (source.isPlaying)
            {
                yield return null;
            }
            DestroySoundInstance();
        }

        IEnumerator UpdateVolume()
        {
            while (gameObject != null)
            {
                if (parentSound.Fading || parentSound.PlaybackMode == SoundPlaybackMode.Granular)
                {
                    source.volume = refVolume * parentSound.FadeFactor;
                }
                yield return null;
            }         
        }

        public void DestroySoundInstance()
        {
            parentSound.PlayingInstances.Remove(this);
            Destroy(this.gameObject);
            parentSound.SendMessage("OnSoundinstanceDestroyed");
        }

        void OnApplicationQuit()
        {
            StopAllCoroutines();
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}

