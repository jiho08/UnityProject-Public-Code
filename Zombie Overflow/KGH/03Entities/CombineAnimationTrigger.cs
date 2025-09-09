using System;
using System.Collections.Generic;
using System.Linq;
using Feedbacks.SFX;
using Hellmade.Sound;
using UnityEngine;

namespace Entities
{
    public class CombineAnimationTrigger : EntityAnimationTrigger, IAfterInit
    {
        [SerializeField] private SoundDataSO footStepSound;
        private List<SplitAnimationTrigger> _splitTrigger;

        public void AfterInit()
        {
            _splitTrigger = GetComponentsInChildren<SplitAnimationTrigger>().ToList();
            foreach (var trigger in _splitTrigger)
            {
                trigger.OnAnimationEndTrigger += AnimationEnd;
                trigger.OnReloadTrigger += ReloadTrigger;
            }
        }

        private void OnDestroy()
        {
            if (_splitTrigger == null) return;
            foreach (var trigger in _splitTrigger)
            {
                trigger.OnAnimationEndTrigger -= AnimationEnd;
                trigger.OnReloadTrigger -= ReloadTrigger;
            }

            _splitTrigger.Clear();
            _splitTrigger = null;
        }

        public void PlayFootStep()
        {
            if (footStepSound == null) return;
            EazySoundManager.PlaySound(footStepSound.GetRandomAudioClip(), footStepSound.volume, footStepSound.loop,
                transform);
        }
    }
}