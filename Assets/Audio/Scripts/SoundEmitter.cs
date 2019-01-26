using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioToolkit
{
    public class SoundEmitter : SoundPlayer
    {
        /// <summary>
        /// The Tag of the ListenerObject.
        /// </summary>
        public string ColliderTag;

        float MaxDistance
        {
            get
            {
                if (_maxDistance == 0f)
                {
                    List<float> _maxDistances = new List<float>();

                    foreach (Sound _sound in Sounds)
                    {
                        _maxDistances.Add(_sound.MaxDistance);
                    }

                    float _max = 0f;

                    foreach (float _distance in _maxDistances)
                    {
                        if (_distance > _max)
                        {
                            _max = _distance;
                        }
                    }

                    _maxDistance =  _max;
                }

                return _maxDistance;               
               }
        }

        float _maxDistance;

        void Start()
        {
            SphereCollider _sphere = gameObject.AddComponent<SphereCollider>();
            _sphere.isTrigger = true;
            _sphere.radius = MaxDistance;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.tag == ColliderTag)
            {
                PlayAll();
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag == ColliderTag)
            {
                foreach (Sound _sound in Sounds)
                {
                    StopAll();
                }
            }
        }

        Sound[] GizmosSounds
        {
            get
            {
                return GetComponents<Sound>();
            }
        }

        float GizmosMaxDistance
        {
            get
            {
                List<float> _maxDistances = new List<float>();

                foreach (Sound _sound in GizmosSounds)
                {
                    _maxDistances.Add(_sound.MaxDistance);
                }

                float _max = 0f;

                foreach (float _distance in _maxDistances)
                {
                    if (_distance > _max)
                    {
                        _max = _distance;
                    }
                }
                return _max;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
            Gizmos.DrawSphere(transform.position, GizmosMaxDistance);
            Gizmos.DrawWireSphere(transform.position, GizmosMaxDistance);
        }
    }
}

