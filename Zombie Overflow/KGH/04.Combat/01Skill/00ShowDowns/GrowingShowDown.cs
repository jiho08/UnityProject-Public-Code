using System;
using Core;
using Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Combat.Skills.ShowDown
{
    public class GrowingShowDown : ShowDownSkill
    {
        [SerializeField] private float maxDistance = 5f;
        public UnityEvent<float> RadiusChangeEvent;
        private float _currentRadius;
        private Collider[] _colliders;

        public override void InitializeSkill(Entity entity, SkillComponent skillComponent)
        {
            base.InitializeSkill(entity, skillComponent);
            _colliders = new Collider[showDownData.maxTargets];
        }

        protected override void UseSkill()
        {
            base.UseSkill();
            _currentRadius = 0f;
            Array.Clear(_colliders, 0, _colliders.Length);
        }

        public override void GetTargets()
        {
            _currentRadius += (maxDistance / showDownData.maxDuration) * Time.unscaledDeltaTime;
            RadiusChangeEvent?.Invoke(_currentRadius);
            Targets.Clear();
            foreach (var col in skillComponent.GetEnemiesInRange(_player.transform, _currentRadius, _colliders))
            {
                if (col != null)
                {
                    Targets.Add(col.transform);
                }
            }
        }

        protected override void ShowDown()
        {
            RadiusChangeEvent?.Invoke(0);
            base.ShowDown();
        }

        public override void CancelSkill()
        {
            RadiusChangeEvent?.Invoke(0);
            base.CancelSkill();
        }
    }
}