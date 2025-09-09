using UnityEngine;
using UnityEngine.Events;

namespace Combat.Skills.ShowDown
{
    public class CircularShowdown : ShowDownSkill
    {
        [SerializeField] private float maxAngle = 180f;
        [SerializeField] private float angleSteps = 10f;
        private float _angle;

        public UnityEvent<float> OnAngleChanged;

        protected override void UseSkill()
        {
            base.UseSkill();
            _angle = 0;
        }

        public override void GetTargets()
        {
            _angle += (maxAngle / showDownData.maxDuration) * Time.unscaledDeltaTime;
            OnAngleChanged?.Invoke(_angle);
        }

        protected override void ShowDown()
        {
            OnShowDown?.Invoke();
            _attackComponent.ShootBurst(_player.transform.forward, angleSteps, _angle, projectileType);
            _attackComponent.ReloadAll();
            OnSkillEnd?.Invoke();
            _angle = 0;
            OnAngleChanged?.Invoke(_angle);
            OnSkillStatusChanged?.Invoke(false);
        }

        public override void CancelSkill()
        {
            _angle = 0;
            OnAngleChanged?.Invoke(_angle);
            base.CancelSkill();
        }
    }
}