using System;
using UnityEngine;

namespace Entities
{
    public class EntityAnimationTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;
        public Action OnAttackTrigger;
        public Action OnJumpStartTrigger;
        public event Action<bool> OnManualRotationTrigger; 
        public event Action<bool> OnManualMoveTrigger;
        public event Action<bool> OnRollingStatusChange;

        public event Action OnAttackVFXTrigger;
        
        public event Action OnReloadTrigger;
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }

        private void Attack() => OnAttackTrigger?.Invoke();
        private void RollingStart() => OnRollingStatusChange?.Invoke(true);
        private void RollingEnd() => OnRollingStatusChange?.Invoke(false);
        private void PlayAttackVfx() => OnAttackVFXTrigger?.Invoke();
        public void ReloadTrigger() => OnReloadTrigger?.Invoke();
        
        private void StartManualRotation() => OnManualRotationTrigger?.Invoke(true);
        private void StopManualRotation() => OnManualRotationTrigger?.Invoke(false); 
        private void StartManualMove() => OnManualMoveTrigger?.Invoke(true);
        private void StopManualMove() => OnManualMoveTrigger?.Invoke(false);

        private void StartJump() => OnJumpStartTrigger?.Invoke();
    }
}