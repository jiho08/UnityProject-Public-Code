using UnityEngine;

namespace Combat.Skills
{
    public abstract class SkillUpgradeDataSO : ScriptableObject
    {
        public int upgradeID;
        public string upgradeName;
        public Sprite upgradeIcon;
        
        [TextArea] public string desc;
        public int maxUpgradeCount;
        
        public abstract void UpgradeSkill(Skill skill);
        public abstract void RollbackUpgrade(Skill skill);
    }
}