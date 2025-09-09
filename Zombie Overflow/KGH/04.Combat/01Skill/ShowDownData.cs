using UnityEngine;

namespace Combat.Skills
{
    [CreateAssetMenu(fileName = "ShowDownData", menuName = "SO/Combat/Skill/ShowDownData", order = 0)]
    public class ShowDownData : ScriptableObject
    {
        public float maxDuration = 2f;
        public int maxTargets = 20;
        
        public float timeMultiplier = 0.5f;
        public int maxSkillEnergy = 2;
        
        public Color fullEnergyColor = Color.yellow;
        public Color energyColor = Color.yellow;
        
        public string showdownClassName;
    }
}