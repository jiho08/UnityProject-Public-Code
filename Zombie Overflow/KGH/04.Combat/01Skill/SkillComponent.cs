using System;
using System.Collections.Generic;
using System.Linq;
using Core.GameEvent;
using Entities;
using UnityEngine;

namespace Combat.Skills
{
    public abstract class SkillComponent : MonoBehaviour, IEntityComponent
    {
        public Skill activeSkill;
        public LayerMask whatIsEnemy;

        protected Entity entity;
        private Dictionary<Type, Skill> _skills;

        public void Initialize(Entity entity)
        {
            this.entity = entity;
            
            _skills = new Dictionary<Type, Skill>();
            GetComponentsInChildren<Skill>().ToList().ForEach(skill => _skills.Add(skill.GetType(), skill));
        }
        
        public bool GetSkill(Type type, out Skill skill)
        {
            if (_skills.TryGetValue(type, out skill))
                return true;

            skill = null;
            return false;
        }
        
        public virtual Collider[] GetEnemiesInRange(Vector3 checkPosition, float range, Collider[] colliders)
        {
            Physics.OverlapSphereNonAlloc(checkPosition, range, colliders, whatIsEnemy);
            return colliders;
        }

        public virtual Collider[] GetEnemiesInRange(Transform checkTransform, float range, Collider[] colliders)
        {
            return GetEnemiesInRange(checkTransform.position, range, colliders);
        }

        public void CancelReleases()
        {
            foreach (var skill in _skills.Values)
            {
                if (skill is IReleasable releasable)
                    releasable.CancelSkill();
            }
        }

        public float SetSkillGauge(float gauge)
        {
            return activeSkill.SetSkillGauge(gauge);
        }
    }
}