using System;
using UnityEngine;

namespace Entities
{
    public class SplitAnimationTrigger : MonoBehaviour
    {
        public Action OnAnimationEndTrigger;
        public Action OnReloadTrigger;

        public void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }
        
        public void ReloadTrigger()
        {
            OnReloadTrigger?.Invoke();
        }
    }
}