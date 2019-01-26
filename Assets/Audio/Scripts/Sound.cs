using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioToolkit
{
    public class Sound : MonoBehaviour
    {
        #region Editor properties

        /// <summary>
        /// The name of this Sound
        /// </summary>
        public string Name = "New Sound";

        /// <summary>
        /// Is this sound a OneShot, Loop or Granular?
        /// </summary>
        public SoundPlaybackMode PlaybackMode;

        /// <summary>
        /// The Prefab containing the AudioSource with the desired attenuation
        /// </summary>
        public GameObject AttenuationPrefab;

        /// <summary>
        /// The OutputMixerGroup this sound is routed to
        /// </summary>
        public AudioMixerGroup Output;

        /// <summary>
        /// The Max Distance attenuation for this sound
        /// </summary>
        public float MaxDistance = 100f;

        /// <summary>
        /// The list containing all the clip that might be played
        /// </summary>
        public List<AudioClip> Clips;

        public float MinDelay = 0f;

        public float MaxDelay = 0f;

        public float FadeInTime = 0f;

        public float FadeOutTime = 0f;

        public float MinVolume = 1f;

        public float MaxVolume = 1f;

        public float MinPitch = 1f;

        public float MaxPitch = 1f;

        public float MinSpawnTime = 0f;

        public float MaxSpawnTime = 0f;

        public float GranularSpawnPositionRandomness = 0f;

        public bool OverridePositionToListener = false;

        public bool RandomStartPosition = false;

        #endregion

        #region Public methods

        /// <summary>
        /// The Parent method for playing this sound. 
        /// Delays the Play() Execution.
        /// </summary>
        public void Play(Vector3 position)
        {
            StartCoroutine(PlayWithDelay(position));
        }

        /// <summary>
        /// The Parent method for stopping this sound
        /// </summary>
        public void Stop()
        {
            stopConfirmed = true;

            // ONE SHOT
            if (PlaybackMode == SoundPlaybackMode.OneShot)
            {
                foreach (SoundInstance _instance in PlayingInstances)
                {
                    _instance.Stop();
                }
            }

            // LOOP
            else if (PlaybackMode == SoundPlaybackMode.Loop)
            {
                FadeOutCoroutine = StartCoroutine(FadeOut());
            }

            // GRANULAR
            else if (PlaybackMode == SoundPlaybackMode.Granular)
            {
                FadeOutCoroutine = StartCoroutine(FadeOut());
            }
        }

        #endregion

        #region Private properties & methods

        IEnumerator PlayWithDelay(Vector3 position)
        {
            yield return new WaitForSeconds(Random.Range(MinDelay, MaxDelay));

            if (stopConfirmed)
            {
                Stop();
            }

            Vector3 _spanwPosition;

            if (!OverridePositionToListener)
            {
                _spanwPosition = position;
            }
            else
            {
                _spanwPosition = AudioManager.Instance.ListenerPosition;
            }

            // ONE SHOT
            if (PlaybackMode == SoundPlaybackMode.OneShot)
            {
                RandomStartPosition = false;

                SoundInstance _instance = SpawnInstance(_spanwPosition).Initialize
                                                    (this, randomClip, false, refVolume, refPitch);
                _instance.Play();

                isPlaying = true;
            }

            // LOOP
            else if (PlaybackMode == SoundPlaybackMode.Loop)
            {
                if (!isPlaying)
                {
                    SoundInstance _instance = SpawnInstance(_spanwPosition).Initialize
                                                        (this, randomClip, true, refVolume, refPitch);
                    _instance.Play();

                }
                FadeInCoroutine = StartCoroutine(FadeIn());
                isPlaying = true;
            }

            // GRANULAR
            else if (PlaybackMode == SoundPlaybackMode.Granular)
            {
                RandomStartPosition = false;

                if (!isPlaying)
                {
                    Granulate();
                }
                FadeInCoroutine = StartCoroutine(FadeIn());
                isPlaying = true;
            }
        }

        SoundInstance SpawnInstance(Vector3 position)
        {
            GameObject _instanceObj = (GameObject)Instantiate(AttenuationPrefab, position, Quaternion.identity);
            _instanceObj.name = "SoundInstance :: " + Name;
            _instanceObj.transform.parent = this.transform;

            SoundInstance _instance = _instanceObj.AddComponent<SoundInstance>();
            return _instance;
        }

        bool stopConfirmed = false;

        AudioClip randomClip
        {
            get
            {
                return Clips[Random.Range(0, Clips.Count)];
            }
        }

        float refVolume
        {
            get
            {
                return Random.Range(MinVolume, MaxVolume);
            }
        }

        float refPitch
        {
            get
            {
                return Random.Range(MinPitch, MaxPitch);
            }
        }

        public List<SoundInstance> PlayingInstances;

        public float FadeFactor
        {
            get
            {
                return fadeFactor;
            }
        }

        public bool Fading;

        public bool isPlaying = false;

        Coroutine FadeInCoroutine;

        Coroutine FadeOutCoroutine;

        float fadeFactor = 0f;

        public void DestroyInstances()
        {
            if (!stopConfirmed)
            {
                return;
            }
            foreach (SoundInstance _instance in PlayingInstances)
            {
                Destroy(_instance.gameObject);
            }
            isPlaying = false;
            PlayingInstances.Clear();
        }

        void Granulate()
        {
            RandomStartPosition = false;

            SoundInstance _instance = SpawnInstance(GranularSpawnPosition()).Initialize
                                                (this, randomClip, false, refVolume, refPitch);
            _instance.Play();

            Invoke("Granulate", Random.Range(MinSpawnTime, MaxSpawnTime));
        }

        Vector3 GranularSpawnPosition()
        {
            // The max distance randomness in world units
            float _maxRandomDistance = MaxDistance * GranularSpawnPositionRandomness;

            // Random Offset Vector3 values
            float _randomX = Random.Range(0f, _maxRandomDistance);
            float _randomZ = Random.Range(0f, _maxRandomDistance - _randomX);
            float _randomY = Random.Range(0f, _maxRandomDistance - _randomX - _randomZ);
            Vector3 _offsetPosition = new Vector3
                (
                Random.Range(-_randomX, _randomX),
                Random.Range(-_randomY, _randomY),
                Random.Range(-_randomZ, _randomZ)
                );

            if (OverridePositionToListener)
            {
                return AudioManager.Instance.ListenerPosition + _offsetPosition;
            }
            return transform.position + _offsetPosition;
        }

        IEnumerator FadeIn()
        {
            if (FadeOutCoroutine != null)
            {
                StopCoroutine(FadeOutCoroutine);
            }

            stopConfirmed = false;

            while (fadeFactor < 1f)
            {
                Fading = true;
                fadeFactor += Time.deltaTime * (1 / FadeInTime);

                if (fadeFactor >= 1f)
                    fadeFactor = 1f;

                yield return null;
            }
            Fading = false;
            StopCoroutine(FadeInCoroutine);
        }

        IEnumerator FadeOut()
        {           
            if (FadeInCoroutine != null)
            {
                StopCoroutine(FadeInCoroutine);
            }

            while (fadeFactor > 0f)
            {
                Fading = true;
                fadeFactor -= Time.deltaTime * (1 / FadeOutTime);

                if (fadeFactor <= 0f)
                    fadeFactor = 0f;

                yield return null;
            }

            Fading = false;
            isPlaying = false;
            DestroyInstances();
            CancelInvoke();
            StopAllCoroutines();           
        }

        void OnSoundinstanceDestroyed()
        {
            if (PlaybackMode == SoundPlaybackMode.OneShot)
            {
                if (PlayingInstances.Count == 0)
                {
                    isPlaying = false;
                }
            }
        }

        #endregion
    }

    public enum SoundPlaybackMode
    {
        OneShot,
        Loop,
        Granular
    }

}
