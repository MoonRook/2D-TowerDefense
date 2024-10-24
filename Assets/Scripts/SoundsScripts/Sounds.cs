using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefense
{
    [CreateAssetMenu()]
    public class Sounds : ScriptableObject
    {
        [SerializeField] private List<SoundByType> _soundByTypes;

        private Dictionary<Sound, AudioClip> _dictionarySounds = new();

        public AudioClip GetClip(Sound key)
        {
            if (_dictionarySounds.TryGetValue(key, out AudioClip clipOld))
            {
                return clipOld;
            }

            AudioClip clip = _soundByTypes.First(sound => sound.Type == key).Clip;

            _dictionarySounds.Add(key, clip);
            return clip;
        }
    }
}

