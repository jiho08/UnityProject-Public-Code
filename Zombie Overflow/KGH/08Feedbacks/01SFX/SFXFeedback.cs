using System;
using Core.Define;
using Hellmade.Sound;
using UnityEngine;

namespace Feedbacks.SFX
{
    public class SFXFeedback : MonoBehaviour, IFeedback
    {
        public SoundDataSO soundData;

        public void PlayFeedback(Transform trm = null)
        {
            switch (soundData.soundType)
            {
                case Audio.AudioType.Music:
                    EazySoundManager.PlayMusic(soundData.GetRandomAudioClip(), soundData.GetRandomVolume(), soundData.loop, soundData.persist);
                    break;
                case Audio.AudioType.Sound:
                    EazySoundManager.PlaySound(soundData.GetRandomAudioClip(), soundData.GetRandomVolume(), soundData.loop);
                    break;
                case Audio.AudioType.UISound:
                    EazySoundManager.PlayUISound(soundData.GetRandomAudioClip(), soundData.GetRandomVolume());
                    break;
                
                    
            }
        }
    }
}