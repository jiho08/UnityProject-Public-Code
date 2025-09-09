using System;
using Core.Define;
using Feedbacks.SFX;
using UnityEngine;

namespace Players
{
    public class PlayerSFXFeedback : SFXFeedback, IPlayerComponent
    {
        [SerializeField] private EnumDefine.PlayerSoundType playerSoundType;
        public void SetUpPlayer(CharacterSO character)
        {
            switch (playerSoundType)
            {
                case EnumDefine.PlayerSoundType.Shoot:
                    soundData = character.gunData.shootSound;
                    break;
                case EnumDefine.PlayerSoundType.Reload:
                    soundData = character.gunData.reloadSound;
                    break;
                case EnumDefine.PlayerSoundType.Empty:
                    soundData = character.gunData.emptySound;
                    break;
                case EnumDefine.PlayerSoundType.Dead:
                    soundData = character.deadSound;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}