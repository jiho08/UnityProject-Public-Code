using System;
using Combat.Skills;
using Combat.Skills.ShowDown;
using Players;
using UnityEngine;

namespace Players.Combat
{
    public class PlayerSkillComponent : SkillComponent, IPlayerComponent
    {
        public void SetUpPlayer(CharacterSO character)
        {
            var skillClass = character.showDownData.showdownClassName;
            var skillType = Type.GetType(skillClass);
            if (GetSkill(skillType, out var skill))
            {
                SetActiveSkill(skill, character);
            }
            else
            {
                Debug.LogError($"Skill of type {skillClass} not found.");
            }
        }
        
        public void SetActiveSkill(Skill skill, CharacterSO character)
        {
            activeSkill = skill;
            activeSkill.InitializeSkill(entity, this);
            if (activeSkill is ShowDownSkill showDownSkill) 
                showDownSkill.showDownData = character.showDownData;
        }
    }
}