using System;
using System.Collections.Generic;
using Animations;
using Core;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        public Action OnSkillEnd;
        public List<Transform> Targets { get; private set; } = new List<Transform>();
        public int remainSkillEnergy;
        
        protected Entity entity;
        protected SkillComponent skillComponent;

        public virtual void InitializeSkill(Entity entity, SkillComponent skillComponent)
        {
            this.entity = entity;
            this.skillComponent = skillComponent;
        }
        
        public virtual bool AttemptUseSkill()
        {
            if (remainSkillEnergy <= 0) return false;
            UseSkill();
            return true;
        }

        protected virtual void UseSkill()
        {
            Targets.Clear();
        }

        public virtual float SetSkillGauge(float gauge)
        {
            remainSkillEnergy = (int)gauge;
            return gauge;
        }
    }
}