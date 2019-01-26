using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AudioToolkit
{
    public class SoundPlayer : MonoBehaviour
    {
        /// <summary>
        /// Returns all the Sound components on this object
        /// </summary>
        public Sound[] Sounds
        {
            get
            {
                if (sounds == null)
                {
                    sounds = gameObject.GetComponents<Sound>();
                }
                return sounds;
            }
        }

        Sound[] sounds;

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

        /// <summary>
        /// Plays a Sound component
        /// </summary>
        /// <param name="soundName">The name of the Sound component to Play</param>
        public void Play(string soundName)
        {
            Sound _sound = GetSound(soundName);
            _sound.Play(Vector3.zero);
        }

        /// <summary>
        /// Play all the Sound components on the object
        /// </summary>
        public void PlayAll()
        {
            foreach (Sound _sound in Sounds)
            {
                _sound.Play(Vector3.zero);
            }
        }

        /// <summary>
        /// Stops a Sound component
        /// </summary>
        /// <param name="soundName">The name of the Sound component to stop</param>
        public void Stop(string soundName)
        {
            Sound _sound = GetSound(soundName);
            _sound.Stop();
        }

        /// <summary>
        /// Stop All the playing Sound components on this Object
        /// </summary>
        public void StopAll()
        {
            foreach (Sound _sound in Sounds)
            {
                _sound.Stop();
            }
        }

        Sound GetSound(string soundName)
        {
            foreach (Sound _sound in Sounds)
            {
                if (_sound.Name == soundName)
                {
                    return _sound;
                }
            }
            Debug.LogError("AudioToolkit :: No sound with name => " + soundName);
            return null;
        }
    }

}
