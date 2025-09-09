using System.Collections.Generic;
using Core;
using Core.Define;
using Hellmade.Sound;
using UnityEngine;

namespace Feedbacks.SFX
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "SO/Sound/SoundData", order = 0)]
    public class SoundDataSO : ScriptableObject
    {
        public List<AudioClip> audioClips;
        public Audio.AudioType soundType;
        public float volume = 1f;
        public float volumeRandomRange = 0;
        public bool loop;
        public bool persist;

        public AudioClip GetRandomAudioClip()
        {
            if (audioClips == null || audioClips.Count == 0)
            {
                Debug.LogWarning("No audio clips available in SoundDataSO.");
                return null;
            }
            return audioClips[Random.Range(0, audioClips.Count)];
        }
        public float GetRandomVolume()
        {
            return Random.Range(volume - volumeRandomRange, volume + volumeRandomRange);
        }
    }
}